using System;

namespace RedPanda.Service.Governance.Configuration
{
    public static class ConfigurationValueExtension
    {
        public static T As<T>( this string value)
        {
            var type = typeof(T);
            T returnValue;

            try
            {
                if (type.IsEnum)
                {
                    returnValue = (T)Enum.Parse(type, value, true);
                }
                else
                {
                    returnValue = (T)Convert.ChangeType(value, type);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Unabale to convert setting value ({value}) to type ({type.FullName}).", e);
            }

            return returnValue;
        }
    }
}