using System;
using UnityEngine;
using UnityEngine.Events;

namespace GablerGames.Core.Converters
{
    public class ConverterListener : MonoBehaviour
    {
        object val;
        object Value
        {
            get => val; set
            {
                val = value;

                var f = val as float?;
                if (f != null)
                    convertToFloat?.Invoke(f.Value);

                var i = Convert.ToInt32(val);
                convertToInt?.Invoke(i);

                convertToString?.Invoke(val.ToString());
            }
        }

        public UnityEvent<float> convertToFloat;
        public UnityEvent<int> convertToInt;
        public UnityEvent<string> convertToString;

        public void SetValue(int val) { Value = val; }
        public void SetValue(float val) { Value = val; }
        public void SetValue(string val) { Value = val; }
    }
}
