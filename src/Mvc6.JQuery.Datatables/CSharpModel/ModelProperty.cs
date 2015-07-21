using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Mvc.JQuery.Datatables.CSharpModel
{
    public class ModelProperty
    {
        public ModelProperty(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Name = PropertyInfo.Name;
            Type = PropertyInfo.PropertyType;
            DisplayAttribute = propertyInfo.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();
            DisplayFormatAttribute = propertyInfo.GetCustomAttributes<DisplayFormatAttribute>().FirstOrDefault();
            DefaultValueAttribute = propertyInfo.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault();
            var attributes = propertyInfo.CustomAttributes;
            //validation
            var maxLengthAttribute = attributes.OfType<MaxLengthAttribute>().FirstOrDefault();//use
            var minLengthAttribute = attributes.OfType<MinLengthAttribute>().FirstOrDefault();//use
            var stringLengthAttribute = attributes.OfType<StringLengthAttribute>().FirstOrDefault();//use validation
            var rangeAttribute = attributes.OfType<RangeAttribute>().FirstOrDefault();//use
            var requiredAttribute = attributes.OfType<RequiredAttribute>().FirstOrDefault();//use validation
            var regularExpressionAttribute = attributes.OfType<RegularExpressionAttribute>().FirstOrDefault();//use
            var dataTypeAttribute = attributes.OfType<DataTypeAttribute>().FirstOrDefault(); //use validation
            var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();//use (name,short,description,prompt,
            //display
            var defaultValueAttribute = attributes.OfType<DefaultValueAttribute>().FirstOrDefault(); //use 
            var displayColumnAttribute = attributes.OfType<DisplayColumnAttribute>().FirstOrDefault();//use for select en combo
            var displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();//use format van date, int etc
            var hiddenInputAttribute = attributes.OfType<HiddenInputAttribute>().FirstOrDefault();//use hide column
            var scaffoldColumnAttribute = attributes.OfType<ScaffoldColumnAttribute>().FirstOrDefault();//use not on form
            var editableAttribute = attributes.OfType<EditableAttribute>().FirstOrDefault();//use readonly
            //skip
            var uiHintAttribute = attributes.OfType<UIHintAttribute>().FirstOrDefault();//skip
            var actionName = attributes.OfType<ActionNameAttribute>().FirstOrDefault(); //skip
            var bindingBehavior = attributes.OfType<BindingBehaviorAttribute>().FirstOrDefault();//skip
            var defaultMemberAttribute = attributes.OfType<DefaultMemberAttribute>().FirstOrDefault();//skip

        }
        
        public PropertyInfo PropertyInfo { get; }
        public Type Type { get; private set; }
        public DisplayAttribute DisplayAttribute { get; }
        public CompareAttribute CompareAttribute { get; }
        public CreditCardAttribute CreditCardAttribute { get; }
        public CustomValidationAttribute CustomValidationAttribute { get; }
        public DataTypeAttribute DataTypeAttribute { get; }
        public DefaultValueAttribute DefaultValueAttribute { get; }
        public DisplayFormatAttribute DisplayFormatAttribute { get; }
        public string Name { get; private set; }
        public object GetValue(object obj, object[] index)
        {
            return PropertyInfo.GetValue(obj, index);
        }
        public void SetValue(object obj, object value)
        {
            PropertyInfo.SetValue(obj, value);
        }
        public string Dummy { get; }
    }
}
public class DefaultValueAttribute : Attribute
{
    public object Value { get; set; }
}