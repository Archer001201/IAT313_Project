using UnityEngine;

namespace Player
{
    public class PlayerAttributes : MonoBehaviour
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
