using System;
using System.Diagnostics;
using System.Reflection;

namespace Common
{
    public class ReflectionHelper
    {
        public static T GetCustomAttribute<T>(Type sourceType) where T : Attribute
        {
            try
            {
                if (sourceType != null)
                {
                    return (T)sourceType.GetCustomAttribute(typeof(T));
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            
            return null;
        }  
    }
}