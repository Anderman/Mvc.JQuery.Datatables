using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;


namespace Mvc.JQuery.Datatables.CSharpModel
{
    public static class ModelProperties<T>
    {
        static ConcurrentDictionary<Type, ModelProperty[]> _propertiesCache = new ConcurrentDictionary<Type, ModelProperty[]>();
        public static ModelProperty[] Properties { get; private set; }

        static ModelProperties()
        {
            Properties = GetPropertiesAndAttributes(typeof(T));
        }
        public static ModelProperty[] GetPropertiesAndAttributes(Type type)
        {
            return _propertiesCache.GetOrAdd(type, t =>
            {
                var infos = from pi in type.GetProperties()
                            select new ModelProperty(pi);
                return infos.ToArray();
            });
        }
    }
}
