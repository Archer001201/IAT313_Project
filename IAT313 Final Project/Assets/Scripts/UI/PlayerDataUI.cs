using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerDataUI : MonoBehaviour
    {
        public PlayerData_SO playerData;
        
        [SerializeField] private Image stressFill;
        [SerializeField] private Image loveFill;
        [SerializeField] private Image studyFill;
        [SerializeField] private GameObject actionBar;

        private GameObject _actionIcon;
        
        private void Awake()
        {
            playerData = Resources.Load<PlayerData_SO>("Data_SO/PlayerData_SO");
            _actionIcon = Resources.Load<GameObject>("Prefabs/Action");
            for (int i = 0; i < playerData.actionPoint; i++)
            {
                Instantiate(_actionIcon, new Vector3(0, 0, 0), Quaternion.identity, actionBar.transform);
            }
        }

        private void OnEnable()
        {
            EventHandler.OnCostActionPoint += () =>
            {
                if (playerData.actionPoint <= 0) return;
                Destroy(actionBar.transform.GetChild(0).gameObject);
                playerData.actionPoint -= 1;
            };
        }

        private void Update()
        {
            stressFill.fillAmount = playerData.stress/10;
            loveFill.fillAmount = playerData.love/10;
            studyFill.fillAmount = playerData.study/10;
        }
        
        
    }
}
