using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventHandler = Utilities.EventHandler;

namespace UI
{
    public class PlayerDataUI : MonoBehaviour
    {
        public PlayerData_SO playerData;
        
        [SerializeField] private Image stressFill;
        [SerializeField] private Image loveFill;
        [SerializeField] private Image studyFill;
        [SerializeField] private GameObject actionBar;
        [SerializeField] private GameObject endLevelPanel;
        [SerializeField] private TextMeshProUGUI sceneName;

        private GameObject _actionIcon;
        
        private void Awake()
        {
            playerData = Resources.Load<PlayerData_SO>("Data_SO/PlayerData_SO");
            _actionIcon = Resources.Load<GameObject>("Prefabs/Action");
            
            if (playerData.actionPoint < 0) playerData.actionPoint = 0;
            if (playerData.actionPoint > playerData.maxActionPoint) playerData.actionPoint = playerData.maxActionPoint;
            
            for (int i = 0; i < playerData.actionPoint; i++)
            {
                Instantiate(_actionIcon, new Vector3(0, 0, 0), Quaternion.identity, actionBar.transform);
            }
        }

        private void OnEnable()
        {
            EventHandler.OnCostActionPoint += HandleCostActionPoint;
        }

        private void OnDisable()
        {
            EventHandler.OnCostActionPoint -= HandleCostActionPoint;
        }

        private void Start()
        {
            sceneName.text = SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
            stressFill.fillAmount = playerData.stress/playerData.maxValue;
            loveFill.fillAmount = playerData.love/playerData.maxValue;
            studyFill.fillAmount = playerData.study/playerData.maxValue;
            endLevelPanel.SetActive(playerData.actionPoint < 1);

            if (playerData.stress < 0) playerData.stress = 0;
            if (playerData.stress > playerData.maxValue) playerData.stress = playerData.maxValue;
            
            if (playerData.study < 0) playerData.study = 0;
            if (playerData.study > playerData.maxValue) playerData.study = playerData.maxValue;
            
            if (playerData.love < 0) playerData.love = 0;
            if (playerData.love > playerData.maxValue) playerData.love = playerData.maxValue;

            if (playerData.actionPoint < 0) playerData.actionPoint = 0;
            if (playerData.actionPoint > playerData.maxActionPoint) playerData.actionPoint = playerData.maxActionPoint;
        }

        private void HandleCostActionPoint()
        {
            if (playerData.actionPoint <= 0) return;
            Destroy(actionBar.transform.GetChild(0).gameObject);
            playerData.actionPoint -= 1;
        }
    }
}
