using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SiteDownChecker.Models
{
    public abstract class ApiModel
    {
        private Type _type;
        private bool _allPropsStored;
        private Dictionary<string, PropertyInfo> _properties;

        //слегка заморочился с оптимизацией
        protected string MakeStringFromProperties(params string[] paramNames)
        {
            _type ??= this.GetType();
            _properties ??= new Dictionary<string, PropertyInfo>();

            var resultBuilder = new StringBuilder();
            if (paramNames.Length > 0)
                foreach (var name in paramNames)
                {
                    var property = _properties.ContainsKey(name)
                        ? _properties[name] = _type.GetProperty(name)
                        : _properties[name];
                    AppendNameAndValue(resultBuilder, property);
                }
            else
            {
                if (!_allPropsStored)
                {
                    foreach (var property in _type.GetProperties())
                        _properties[property.Name] = property;
                    _allPropsStored = true;
                }

                foreach (var property in _properties.Values)
                    AppendNameAndValue(resultBuilder, property);
            }

            resultBuilder.Remove(resultBuilder.Length - 2, 2);

            return resultBuilder.ToString();

            void AppendNameAndValue(StringBuilder builder, PropertyInfo property) =>
                builder.Append($"{property.Name}: {HandleNull(property.GetValue(this))}, ");

            static string HandleNull(object obj) =>
                obj is null ? "{NULL}" : obj.ToString();
        }
    }
}
