using System;
using System.Reflection;

namespace Mvc.JQuery.Datatables.CSharpModel
{
    public class ModelProperty
    {
        public ModelProperty(PropertyInfo propertyInfo, Attribute[] attributes)
        {
            PropertyInfo = propertyInfo;
            Name = PropertyInfo.Name;
            Type = PropertyInfo.PropertyType;
        }

        public PropertyInfo PropertyInfo { get; }
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public object GetValue(object obj, object[] index)
        {
            return PropertyInfo.GetValue(obj, index);
        }
        public void SetValue(object obj, object value)
        {
            PropertyInfo.SetValue(obj,value);
        }
    }
}