using System;
using ScriptableObjects;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerDataUI : MonoBehaviour
    {
        // public float stress;
        // public float love;
        // public float study;
        public PlayerData_SO playerData;
        
        [SerializeField] private Image stressFill;
        [SerializeField] private Image loveFill;
        [SerializeField] private Image studyFill;
        //
        // private void OnEnable()
        // {
        //     EventHandler.OnAfterEventEffect += floats =>
        //     {
        //         stress += floats[0];
        //         love += floats[1];
        //         study += floats[2];
        //     };
        // }

        private void Awake()
        {
            playerData = Resources.Load<PlayerData_SO>("Data_SO/PlayerData_SO");
        }

        private void Update()
        {
            stressFill.fillAmount = playerData.stress/10;
            loveFill.fillAmount = playerData.love/10;
            studyFill.fillAmount = playerData.study/10;
        }
    }
}
