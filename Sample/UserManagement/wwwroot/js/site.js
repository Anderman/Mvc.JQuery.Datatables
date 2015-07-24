
//This script is the same for each jquery-datatable.
//Define your own functions to extend the column rendering or custom error/succes message

var mvc = { "JQuery": { "Datatables": { ajax: {}, tableTools: {}, column: {} } } };

mvc.JQuery.Datatables.column.createMailToLink = function (data, type, full, meta) {
    return '<a href=mailto:' + data + '>' + data + '</a>';
}
mvc.JQuery.Datatables.column.formatDate = function (data, type, full, meta) {
    return data ? moment(data).format(meta.settings.aoColumns[meta.col].mvc6Par || "YY-DD-mm hh:mm:ss") : null;
}
mvc.JQuery.Datatables.returnNull = function (data) { return null; }
mvc.JQuery.Datatables.getLengthMenu = function () { return [[5, 50, 100, -1], [5, 50, 100, "All"]] };
mvc.JQuery.Datatables.getLanguage = function () { return {} };
mvc.JQuery.Datatables.ajax.success = function (data, textStatus, jqXHR) { };
mvc.JQuery.Datatables.ajax.error = function (jqXHR, textStatus, errorThrown) { alert(errorThrown) };
mvc.JQuery.Datatables.ajax.load = function (response, textStatus, xhr) {
    if (textStatus === 'error')
        alert(xhr.status + " " + xhr.statusText);

};
mvc.JQuery.Datatables.tableTools.CUD = function (connect) {
    return {
        sRowSelect: "os",
        aButtons: [
            {
                sRowSelect: "os",
                sExtends: "select_single",
                sButtonClass: "waves-effect waves-light disabled modal-trigger",
                sButtonText: "Edit",
                fnInit: function () {
                    connect.tableTools = this;
                },
                fnClick: function (nButton, oConfig) {
                    if (this.fnGetSelected().length === 1) {
                        var tableTools = this;
                        var id = this.fnGetSelectedData()[0].Id;
                        $("#myModal").ajaxForm({
                            url: '/User/Edit/' + id,
                            dataChanged: function () {
                                tableTools.fnSelectNone();//Fix tabletool bug to disable buttons when no row is selected
                                connect.datatable.fnDraw(false);//redraw the table to show the new data
                            }
                        });
                    }
                }
            }
        ]
    }
}

// Bind every table with the class datatable
// Read all information that can be used for data request from the table and th tags
$(document).ready(function () {
    $('table.datatables').each(function () {
        var connect = {};
        var $datatable = $(this);
        var getRenderInfo = function () {
            var columns = [];
            $($datatable.find("thead > tr > th[data-data]")).each(function () {
                var column = {};
                var render = $(this).attr("data-render");
                if (render)
                    if (render.substring(0, 1) === "[")
                        column.render = render;
                    else
                        column.render = (mvc.JQuery.Datatables.column[render]);
                columns.push(column);
            });
            return columns;
        };
        var root = {
            processing: true,
            serverSide: true,
            fnServerData: function (sSource, aoData, fnCallback) {
                for (var dataTableRequest = {}, i = 0 ; i < aoData.length; i++) dataTableRequest[aoData[i].name] = aoData[i].value;
                $.ajax({
                    url: $(this).attr("data-url"),
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataTableRequest),
                    success: function (data, textStatus, jqXHR) {
                        if (connect.tableTools)//Fix tabletool bug to disable buttons when no row is selected
                            connect.tableTools.fnSelectNone();
                        (mvc.JQuery.Datatables.ajax[$(this).attr("data-succes") || "success"])(data, textStatus, jqXHR);
                        fnCallback(data, textStatus, jqXHR);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        (mvc.JQuery.Datatables.ajax[$(this).attr("data-error") || "error"])(jqXHR, textStatus, errorThrown);
                    },
                });
            },
            columns: getRenderInfo(),
            language: (mvc.JQuery.Datatables[$(this).attr("data-language") || "returnNull"])(),
            lengthMenu: (mvc.JQuery.Datatables[$(this).attr("data-lengthMenu") || "returnNull"])(),
            tableTools: mvc.JQuery.Datatables.tableTools[$(this).attr("data-tableTools")](connect)
        }
        connect.datatable = $(this).dataTable(root);//give tabletool access to datatable to redraw after edit
    });
});
jQuery.fn.extend({
    ajaxForm: function (options) {
        var modal = this;
        var $form = null;
        var initModelForm = function () {
            $form.areYouSure({
                change: function () {
                    var form = this;
                    // Enable save button only if the form is dirty. i.e. something to save.
                    if ($(form).hasClass('dirty')) {
                        $(form).find('[type="submit"]').removeClass('disabled').removeAttr('disabled');;
                    } else {
                        $(form).find('[type="submit"]').addClass('disabled').attr('disabled', 'disabled');;
                    }
                },
                'message': 'De wijzigingen zijn niet opgeslagen!'
            });
        };
        $(modal).load(options.url, function (response, textStatus, xhr) {
            if (textStatus === 'error')
                alert(xhr.status + " " + xhr.statusText);
            else {
                $.validator.unobtrusive.parse(modal);
                $(modal).openModal({
                    dismissible: false
                });
                $form = $(modal).find('form');
                initModelForm();

                $form.submit(function (ev) {
                    $.ajax({
                        type: $form.attr('method'),
                        url: $form.attr('action'),
                        data: $form.serialize(),
                        success: function (data) {
                            if (data) {
                                $(modal).find('.modal-close').off('click.close');
                                $(modal).html(data);
                                $(modal).find(".modal-close").on('click.close', function (e) {
                                    $(modal).closeModal();
                                });
                            } else {
                                $(modal).closeModal();
                                if (typeof (options.dataChanged) === "function")
                                    options.dataChanged();
                            }
                        },
                        error: mvc.JQuery.Datatables.ajax.error
                    });
                    ev.preventDefault();
                });
            }
        });
    }

});
