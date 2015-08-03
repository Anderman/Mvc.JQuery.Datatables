﻿
//This script is the same for each jquery-datatable.
//Define your own functions to extend the column rendering or custom error/succes message

var mvc = { 'JQuery': { 'Datatables': { ajax: {}, tableTools: {}, column: {} } } };

mvc.JQuery.Datatables.column.createMailToLink = function (data, type, full, meta) {
    return '<a href=mailto:' + data + '>' + data + '</a>';
}
mvc.JQuery.Datatables.column.formatDate = function (data, type, full, meta) {
    return data ? window.moment(data).format(meta.settings.aoColumns[meta.col].mvc6Par || 'YY-DD-mm hh:mm:ss') : null;
}
mvc.JQuery.Datatables.returnNull = function (data) { return null; }
mvc.JQuery.Datatables.getLengthMenu = function () { return [[5, 50, 100, -1], [5, 50, 100, 'All']] };
mvc.JQuery.Datatables.getLanguage = function () { return {} };
mvc.JQuery.Datatables.ajax.success = function (data, textStatus, jqXHR) { };
mvc.JQuery.Datatables.ajax.error = function(jqXHR, textStatus, errorThrown) {
    var newWindow = window.open();
    newWindow.document.write(jqXHR.responseText);
};
mvc.JQuery.Datatables.ajax.load = function (response, textStatus, xhr) {
    if (textStatus === 'error')
        alert(xhr.status + ' ' + xhr.statusText);
};

// Bind every table with the class datatable
$(document).ready(function () {
    $('table.datatables').each(function () {
        $(this).MvcDatatable({ leaveMessage: 'De wijzigingen zijn niet opgeslagen! toch door gaan?' });
    });


    //$(".form_datetime").datetimepicker({
    //    format: "dd-mm-yyyy hh:ii",
    //    autoclose: true,
    //    todayBtn: true
    //});

    //$(function () {
    //$('#datetimepicker1').datetimepicker({ autoclose: true });
    //$('#datetimepicker2').datetimepicker({ autoclose: true });
    //$('.form_datetime3').datetimepicker({ autoclose: true });

    //$(document).on('hidden.bs.modal', function (e) {
    //    //$modal.off('click.confirm');
    //    //$modal.off('hide.bs.modal');
    //    //$modal.off('hidden.bs.modal');
    //    $('body').addClass('modal-open');
    //},'.modal');
    //$(document).on(
    //    'hidden.bs.modal', function () {
    //        if ($('.modal:visible').length > 0) {setTimeout(function () {$(document.body).addClass('modal-open');}, 0);}
    //    }
    //, '.modal');
    //$(function () {
    //    $('#datetimepicker1').datetimepicker();
    //});

});

jQuery.fn.extend({
    //todo muti-delete 
    //todo CRUD without tabletools
    MvcDatatable: function (options) {
        var connect = {};//connect give tabletool access to datatable to redraw the table after save form and datatable access to tabletools to fix disable button bug
        var $datatable = $(this);
        var createLink = $datatable.attr('data-create');
        var editLink = $datatable.attr('data-edit');
        var deleteLink = $datatable.attr('data-delete');
        var getRenderInfo = function () {
            var columns = [];
            $($datatable.find('thead > tr > th[data-data]')).each(function () {
                var column = {};
                var render = $(this).attr('data-render');
                if (render)
                    if (render.substring(0, 1) === '[')
                        column.render = render;
                    else
                        column.render = (mvc.JQuery.Datatables.column[render]);
                columns.push(column);
            });
            return columns;
        };
        var loadAjaxTabledata = function (sSource, aoData, fnCallback) {
            for (var dataTableRequest = {}, i = 0 ; i < aoData.length; i++) dataTableRequest[aoData[i].name] = aoData[i].value;
            $.ajax({
                url: $(this).attr('data-url'),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(dataTableRequest),
                success: function (data, textStatus, jqXHR) {
                    if (connect.tableTools)//Fix tabletool bug to disable buttons when no row is selected
                        connect.tableTools.fnSelectNone();
                    (mvc.JQuery.Datatables.ajax[$(this).attr('data-succes') || 'success'])(data, textStatus, jqXHR);
                    fnCallback(data, textStatus, jqXHR);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    (mvc.JQuery.Datatables.ajax[$(this).attr('data-error') || 'error'])(jqXHR, textStatus, errorThrown);
                }
            });
        };
        var editButton = {
            sExtends: 'select_single',
            sButtonClass: 'btn btn-primary btn-raised disabled',
            sButtonText: 'Edit',
            fnInit: function () {
                connect.tableTools = this;
            },
            fnClick: function (nButton, oConfig) {
                if (this.fnGetSelected().length === 1) {
                    var id = this.fnGetSelectedData()[0].Id;
                    $('#ajaxForm').ajaxForm({
                        url: editLink + id,
                        leaveMessage: options.leaveMessage,
                        dataChanged: function () {
                            connect.tableTools.fnSelectNone();//Fix tabletool bug to disable buttons when no row is selected
                            connect.datatable.fnDraw(false);//redraw the table to show the new data
                        }
                    });
                }
            }
        }
        var createButton = {
            sExtends: 'text',
            sButtonClass: 'btn btn-primary btn-raised modal-trigger',
            sButtonText: 'Create',
            fnInit: function () {
                connect.tableTools = this;
            },
            fnClick: function (nButton, oConfig) {
                $('#ajaxForm').ajaxForm({
                    url: createLink,
                    leaveMessage: options.leaveMessage,
                    dataChanged: function () {
                        connect.tableTools.fnSelectNone();//Fix tabletool bug to disable buttons when no row is selected
                        connect.datatable.fnDraw(false);//redraw the table to show the new data
                    }
                });
            }
        }
        var deleteButton = {
            sExtends: 'select_single',
            sButtonClass: 'btn btn-primary btn-raised disabled',
            sButtonText: 'Delete',
            fnInit: function () {
                connect.tableTools = this;
            },
            fnClick: function (nButton, oConfig) {
                if (this.fnGetSelected().length >= 1) {
                    var id = this.fnGetSelectedData()[0].Id;
                    $('#ajaxForm').ajaxForm({
                        url: deleteLink + id,
                        leaveMessage: options.leaveMessage,
                        dataChanged: function () {
                            connect.tableTools.fnSelectNone(); //Fix tabletool bug to disable buttons when no row is selected
                            connect.datatable.fnDraw(false); //redraw the table to show the new data
                        }
                    });
                }
            }
        }

        var buttons = [];
        if (createLink)
            buttons.push(createButton);
        if (editLink)
            buttons.push(editButton);
        if (deleteLink)
            buttons.push(deleteButton);
        var ttOptions = {
            sRowSelect: 'os',
            aButtons: buttons
        }
        connect.datatable = $(this).dataTable({
            processing: true,
            serverSide: true,
            fnServerData: loadAjaxTabledata,
            columns: getRenderInfo(),
            language: (mvc.JQuery.Datatables[$(this).attr('data-language') || 'returnNull'])(),
            lengthMenu: (mvc.JQuery.Datatables[$(this).attr('data-lengthMenu') || 'returnNull'])(),
            tableTools: ttOptions
        });
    }
});

///Confirm dialog
///input a html diaglog with buttons with the class .btn
///output the buttonname or 'cancel' is esc is pressed
jQuery.fn.extend({
    confirm: function (callback) {
        var $modal = $(this);
        var button = 'cancel';

        $modal.off('click.confirm');
        $modal.off('hide.bs.modal');
        $modal.off('hidden.bs.modal');
        $modal.on('click.confirm', '.btn', function (e) {
            button = this.name;
            $modal.modal('hide');
        });
        $modal.on('hide.bs.modal', function (e) {
            if (typeof (callback) === 'function') {
                callback(button);
            }
        });
        $modal.on('hidden.bs.modal', function (e) {
            $('.modal:visible').length > 0
            && $('body').addClass('modal-open-backup')
            && setTimeout(function () { $('body').addClass('modal-open').removeClass('modal-open-backup'); }, 0);
            //$modal.off('hidden.bs.modal');
            //$('body').addClass('modal-open');
        });
        $modal.modal('show');
        $modal.addClass("in");
        $modal.css({ display: "block" });
    }
});
jQuery.fn.extend({
    ajaxForm: function (options) {
        var modal = this;
        var $form = null;
        var closeForm =function()
        {
            if ($form.hasClass('dirty')) {
                $('#confirm.modal').confirm(function (button) {
                    if (button === 'yes') {
                        $form.submit();
                    }
                    if (button === 'no') {
                        $form.trigger('reinitialize.areYouSure'); //set form not dirty 
                        $(modal).modal('hide');
                    }
                });
            } else {
                $form.trigger('reinitialize.areYouSure');// allow navigation
                $(modal).modal('hide');
            }
        }
        var bindCloseClass = function () {
            $form.on('click.close', '.modal-close', function (e) {
                closeForm();
            });
        }
        var bindBackdrop = function () {
            $(modal).on('click.ajaxform', function (e) {
                if (e.target !== e.currentTarget)
                    return;
                closeForm();
            });
        }
        var bindAreyouSure = function () {
            $form.areYouSure({
                change: function () {
                    // Enable save button only if the form is dirty. i.e. something to save.
                    if ($form.hasClass('dirty')) {
                        $form.find('[type="submit"]').removeClass('disabled').removeAttr('disabled');
                    } else {
                        $form.find('[type="submit"]').addClass('disabled').attr('disabled', 'disabled');;
                    }
                },
                message: options.leaveMessage
            });
        }
        var bindSubmitForm = function () {
            $form.submit(function (ev) {
                if (!$form.valid())
                    return;
                $.ajax({
                    type: $form.attr('method'),
                    url: $form.attr('action'),
                    data: $form.serialize(),
                    success: function (data) {
                        if (data) {
                            initModelForm(data);
                        } else {
                            $(modal).modal('hide');
                            if (typeof (options.dataChanged) === 'function')
                                options.dataChanged();
                        }
                    },
                    error: mvc.JQuery.Datatables.ajax.error
                });
                ev.preventDefault();
            });
        };
        var bindEscape = function () {
            //debugger;
            $('body').on('keyup.dismiss.bs.modal', function (e) {
                if (e.which === 27) {
                    if (!$form.hasClass('dirty'))
                        $(modal).modal('hide');
                    else
                        closeForm();
                }
            });
        }
        var initModelForm = function (data) {
            if (data) {
                $(modal).html(data); //load the new form
            }
            $form = $(modal).find('form');
            $form.off('submit');
            $form.off('click.close', '.modal-close');
            $(modal).off('click.ajaxform');
            $(modal).off('shown.bs.modal.my');
            $('body').off('keyup.dismiss.bs.modal');
            bindSubmitForm();
            bindAreyouSure();
            bindCloseClass();
            bindBackdrop();
            bindEscape();
            $(modal).on('shown.bs.modal.my', function () {
                $form.find('input:enabled').first()[0].focus();
            });
            $.validator.unobtrusive.parse('form');

        };
        $(modal).load(options.url, function (response, textStatus, xhr) {
            if (textStatus === 'error')
                alert(xhr.status + ' ' + xhr.statusText);
            else {
                $(modal).modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
                });
                initModelForm();

            }
        });
    }
});

