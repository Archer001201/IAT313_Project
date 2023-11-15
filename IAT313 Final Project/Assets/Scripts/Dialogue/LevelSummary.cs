using System;
using System.Collections;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Dialogue
{
    public class LevelSummary : MonoBehaviour
    {
        public string jsonFile;
        [SerializeField] private TextMeshProUGUI summaryText;
        [SerializeField] private SummaryInfo summaryInfo;

        [SerializeField] private LevelData_SO levelData;
        [SerializeField] private PlayerData_SO playerData;
        [SerializeField] private GameObject spaceSign;

        private bool _isFinished;
        private string _nextLevelID;
        private InputControls _inputControls;

        private void Awake()
        {
            _isFinished = false;
            _inputControls = new InputControls();
            _inputControls.GamePlay.ConfirmButton.performed += GoToNextLevel;
            
            LoadJsonFile();
            StartCoroutine(ShowSummaryText());
        }

        private void OnEnable()
        {
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        private void Update()
        {
            spaceSign.SetActive(_isFinished);
        }

        private void LoadJsonFile()
        {
            LevelInfo currentLevel = null;

            foreach (var level in levelData.levels)
            {
                if (!level.levelID.Equals(levelData.currentLevelID)) continue;
                currentLevel = level;
            }
            if (currentLevel == null) return;

            jsonFile = currentLevel.summaryFile;

            TextAsset jsonAsset = Resources.Load<TextAsset>("JSON_Files/Data/" + jsonFile);
            if (jsonAsset != null)
            {
                summaryInfo = JsonUtility.FromJson<SummaryInfo>(jsonAsset.text);
            }
            else
            {
                Debug.LogError(jsonFile + ".json can not be loaded");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ShowSummaryText()
        {
            bool validCondition = false;
            foreach (var branch in summaryInfo.branches)
            {
                bool isSatisfiedStudy = false;
                bool isSatisfiedLove = false;

                if (branch.conditionStudy[0] == 0)
                {
                    if (playerData.study < branch.conditionStudy[1]) isSatisfiedStudy = true;
                }
                else
                {
                    if (playerData.study >= branch.conditionStudy[1]) isSatisfiedStudy = true;
                }
                
                if (branch.conditionLove[0] == 0)
                {
                    if (playerData.love < branch.conditionLove[1]) isSatisfiedLove = true;
                }
                else
                {
                    if (playerData.love >= branch.conditionLove[1]) isSatisfiedLove = true;
                }

                if (!isSatisfiedStudy || !isSatisfiedLove) continue;
                validCondition = true;
                summaryText.text = "";
                yield return summaryText.DOText(branch.branch, 3f).WaitForCompletion();
                _nextLevelID = branch.nextLevel;
                _isFinished = true;
            }
            if (!validCondition) Debug.LogError(jsonFile + ".json has invalid condition");
        }

        private void GoToNextLevel(InputAction.CallbackContext context)
        {
            if (!_isFinished) return;
            Debug.Log("Next Level ID:" + _nextLevelID);
            SceneManager.LoadScene("Home");
            levelData.currentLevelID = _nextLevelID;
            playerData.actionPoint += 3;
        }
    }
}
