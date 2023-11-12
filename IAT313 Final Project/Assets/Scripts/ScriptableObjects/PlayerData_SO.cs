using UnityEngine;
using UnityEngine.PlayerLoop;
using Utilities;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData_SO", menuName = "Scriptable Object/Player Data")]
    // ReSharper disable once InconsistentNaming
    public class PlayerData_SO : ScriptableObject
    {
        public float maxValue;
        public int maxActionPoint;
        public float stress;
        public float love;
        public float study;
        public int actionPoint;
        
        private void OnEnable()
        {
            EventHandler.OnAfterEventEffect += floats =>
            {
                int delta = 0;
                if (stress > maxValue * 0.7 || stress < maxValue*0.3) delta = -1;
                stress += floats[0];
                love += floats[1] + delta;
                study += floats[2] + delta;
            };
        }
    }
}
