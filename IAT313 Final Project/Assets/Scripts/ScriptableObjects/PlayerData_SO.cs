using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData_SO", menuName = "Scriptable Object/Player Data")]
    // ReSharper disable once InconsistentNaming
    public class PlayerData_SO : ScriptableObject
    {
        public float stress;
        public float love;
        public float study;
        
        private void OnEnable()
        {
            EventHandler.OnAfterEventEffect += floats =>
            {
                stress += floats[0];
                love += floats[1];
                study += floats[2];
            };
        }
    }
}
