using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utility
{
    public static class MethodExtensions
    {
       public static string RemoveQuotes(this string value)
        {
            return value.Replace("\"", "");
        }

        public static float OneDecimal(this float value)
        {
             return Mathf.Round(value * 10f) / 10;
        }
    }
}

