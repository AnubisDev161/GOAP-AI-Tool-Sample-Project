using UnityEngine;

namespace GOAP.Core
{
    public static class GOAPDebug
    {
        public static string AddErrorColorAndSize(string errorMessage, float size = 12)
        {
            return $"<size={size}><color=#FF7F7F>{errorMessage}</color></size>";
        }
    }
}
