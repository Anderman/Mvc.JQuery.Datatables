using System;
using System.Collections.Generic;
using Mvc.JQuery.Datatables.CSharpModel;

namespace Mvc.JQuery.Datatables
{
    public class DataTablesWhereBuilder
    {
        private enum SearchType
        {
            contains = 0,
            endsWith = 1,
            startsWith = 2,
            equal = 3
        }
        public List<object> DynamicLinqParameters = new List<object>();
        public List<string> DynamicLinqString = new List<string>();
        public void addFilter(ModelProperty modelProperty, string regSearchValues)
        {
            foreach (var regSearch in regSearchValues.Split('|'))
            {
                var searchType = (SearchType)(regSearch.StartsWith("^") ? 2 : 0) + (regSearch.EndsWith("$") ? 1 : 0);
                var search = regSearch.TrimStart('^').TrimEnd('$');
                if (search == null || search == ".*") return;

                if (modelProperty.Type.IsString()) AddStringFilter(modelProperty, search, searchType);
                if (modelProperty.Type.IsEnum) addEnumFilter(modelProperty, search, searchType);
                if (modelProperty.Type.isBool()) AddBoolFilter(modelProperty, search, searchType);
                if (modelProperty.Type.IsDateTimeType()) AddDateTimeFilter(modelProperty, search, searchType);
                if (modelProperty.Type.IsNumericType()) AddNumericFilter(modelProperty, search, searchType);
                if (modelProperty.Type.IsDateTimeOffsetType()) AddDateTimeOffsetFilter(modelProperty, search, searchType);
            }
        }

        private void AddNumericFilter(ModelProperty modelProperty, string search, SearchType searchType)
        {
            object value = null;
            if (search.Contains("~"))
            {
                var parts = search.Split('~');
                string filterString = null;

                if (TryParseValue(modelProperty.Type, parts[0], out value))
                {
                    filterString = string.Format("{0} >= @{1}", modelProperty.Name, DynamicLinqParameters.Count);
                    DynamicLinqParameters.Add(value);
                }

                if (TryParseValue(modelProperty.Type, parts[1], out value))
                {
                    filterString = string.Format("{0} <= @{1}", modelProperty.Name, DynamicLinqParameters.Count);
                    DynamicLinqParameters.Add(value);
                }
                if (filterString != null)
                    DynamicLinqString.Add(filterString);
            }
            else
            {
                if (TryParseValue(modelProperty.Type, search, out value))
                {
                    DynamicLinqString.Add(string.Format("{0} == @{1}", modelProperty.Name, DynamicLinqParameters.Count));
                    DynamicLinqParameters.Add(value);
                }
            }
        }

        private void AddDateTimeOffsetFilter(ModelProperty modelProperty, string search, SearchType searchType)
        {
            if (search.Contains("~"))
            {
                var parts = search.Split('~');
                string filterString = null;
                DateTimeOffset start, end;

                if (DateTimeOffset.TryParse(parts[0] ?? "", out start))
                {
                    filterString = modelProperty.Name + " >= @" + DynamicLinqParameters.Count;
                    DynamicLinqParameters.Add(start);
                }

                if (parts.Length > 1 && DateTimeOffset.TryParse(parts[1] ?? "", out end))
                {
                    filterString = (filterString == null ? null : filterString + " and ") + modelProperty.Name + " <= @" + DynamicLinqParameters.Count;
                    DynamicLinqParameters.Add(end);
                }
                if (filterString != null)
                    DynamicLinqString.Add(filterString);
            }
            else
            {
                DateTimeOffset dateTimeOffset;
                if (DateTimeOffset.TryParse(search, out dateTimeOffset))
                {
                    DynamicLinqString.Add(modelProperty.Name + " == @" + DynamicLinqParameters.Count);
                    DynamicLinqParameters.Add(dateTimeOffset);
                }
            }

        }

        private void AddDateTimeFilter(ModelProperty modelProperty, string search, SearchType searchType)
        {
            if (search.Contains("~"))
            {
                var parts = search.Split('~');
                string filterString = null;
                DateTime start, end;

                if (DateTime.TryParse(parts[0] ?? "", out start))
                {
                    filterString = modelProperty.Name + " >= @" + DynamicLinqParameters.Count;
                    DynamicLinqParameters.Add(start);
                }

                if (parts.Length > 1 && DateTime.TryParse(parts[1] ?? "", out end))
                {
                    filterString = (filterString == null ? null : filterString + " and ") + modelProperty.Name + " <= @" + DynamicLinqParameters.Count;
                    DynamicLinqParameters.Add(end);
                }
                if (filterString != null)
                    DynamicLinqString.Add(filterString);
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(search, out dateTime))
                {
                    DynamicLinqString.Add(modelProperty.Name + " == @" + DynamicLinqParameters.Count);
                    DynamicLinqParameters.Add(dateTime);
                }
            }

        }

        private void addEnumFilter(ModelProperty modelProperty, string search, SearchType searchType)
        {
            object value = null;
            if (TryParseValue(modelProperty.Type, search, out value))
            {
                DynamicLinqString.Add(modelProperty.Name + " == @" + (DynamicLinqParameters.Count));
                DynamicLinqParameters.Add(value);
            }
        }

        private void AddStringFilter(ModelProperty modelProperty, string search, SearchType searchType)
        {
            var parameterArg = "@" + (DynamicLinqParameters.Count);
            DynamicLinqParameters.Add(search);

            switch (searchType)
            {
                case SearchType.equal:
                    DynamicLinqString.Add(string.Format("{0} ==  {1}", modelProperty.Name, parameterArg));
                    break;
                case SearchType.startsWith:
                    DynamicLinqString.Add(string.Format("({0}.StartsWith({1}))", modelProperty.Name, parameterArg));
                    break;
                case SearchType.endsWith:
                    DynamicLinqString.Add(string.Format("({0}.EndsWith({1}))", modelProperty.Name, parameterArg));
                    break;
                default:
                    DynamicLinqString.Add(string.Format("({0}.Contains({1}))", modelProperty.Name, parameterArg));
                    break;
            }
        }

        private void AddBoolFilter(ModelProperty modelProperty, string search, SearchType searchType)
        {
            search = search.ToLowerInvariant();
            if (search == "false" || search == "true" || (search == "null" && modelProperty.Type == typeof(bool?)))
            {
                DynamicLinqString.Add(string.Format(modelProperty.Name + " == {0}", search));
            }
            return;
        }

        private static bool TryParseValue(Type t, string input, out object value)
        {
            try
            {
                value = ParseValue(t, input);
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
            return true;
        }
        private static object ParseValue(Type t, string input) {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type u = Nullable.GetUnderlyingType(t);
                return Convert.ChangeType(input, u);
            }
            return t.IsEnum? Enum.Parse(t, input) : Convert.ChangeType(input, t);
        }
    }
    public static class TypeExtensions
    {
        public static bool isBool(this Type type) =>
            type == typeof(bool) ||
            type == typeof(bool?);
        public static bool IsNumericType(this Type type)
        {
            if (type == null || type.IsEnum)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }
        public static bool IsString(this Type type) =>
            type == typeof(string);
        public static bool IsEnumType(this Type type) =>
            type.IsEnum;
        public static bool IsBoolType(this Type type) =>
            type == typeof(bool) ||
            type == typeof(bool?);
        public static bool IsDateTimeType(this Type type) =>
            type == typeof(DateTime) ||
            type == typeof(DateTime?);
        public static bool IsDateTimeOffsetType(this Type type) =>
            type == typeof(DateTimeOffset) ||
            type == typeof(DateTimeOffset?);
    }
}