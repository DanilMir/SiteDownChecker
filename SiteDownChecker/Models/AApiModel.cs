using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MedicalVideo.Models
{
    public abstract class ApiModel
    {
        private Type type;
        private bool allPropsStored;
        private Dictionary<string, PropertyInfo> properties;

        //слегка заморочился с оптимизацией
        protected string MakeStringFromProperties(params string[] paramNames)
        {
            type ??= this.GetType();
            properties ??= new Dictionary<string, PropertyInfo>();

            var resultBuilder = new StringBuilder();
            if (paramNames.Length > 0)
                foreach (var name in paramNames)
                {
                    var property = properties.ContainsKey(name)
                        ? properties[name] = type.GetProperty(name)
                        : properties[name];
                    AppendNameAndValue(resultBuilder, property);
                }
            else
            {
                if (!allPropsStored)
                {
                    foreach (var property in type.GetProperties())
                        properties[property.Name] = property;
                    allPropsStored = true;
                }
                foreach (var property in properties.Values)
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