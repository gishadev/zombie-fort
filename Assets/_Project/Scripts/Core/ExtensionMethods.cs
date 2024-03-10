using UnityEngine;

namespace gishadev.fort.Core
{
    public static class ExtensionMethods
    {
        public static float MapValue(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            value = Mathf.Clamp(value, fromMin, fromMax);
            float mappedValue = (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;

            return mappedValue;
        }
    }
}