using _01._Script.Data;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01._Script.UI
{
    public class SaveSlotUI : MonoBehaviour
    {
        [Header("--- Slot Settings ---")]
        [SerializeField] private int slotIndex;
        [SerializeField] private string targetSceneName = "01.Village";

        [Header("--- UI References ---")]
        [SerializeField] private GameObject dataContainer;
        [SerializeField] private GameObject emptyTextObject;
        
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI stoneText;
        [SerializeField] private TextMeshProUGUI weaponLevel;

        [Header("--- Buttons ---")]
        [SerializeField] private Button loadButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button updateButton;

        private void Start()
        {
            RefreshSlot();
            
            if (loadButton != null) loadButton.onClick.AddListener(OnLoadClicked);
            if (saveButton != null) saveButton.onClick.AddListener(OnSaveClicked);
            if (deleteButton != null) deleteButton.onClick.AddListener(OnDeleteClicked);
            if (saveButton != null) updateButton.onClick.AddListener(OnSaveClicked);
        }

        public void RefreshSlot()
        {
            var data = DataManager.Instance.GetSaveDataPreview(slotIndex);

            if (data == null)
            {
                dataContainer.SetActive(false);
                emptyTextObject.SetActive(true);
                if (deleteButton != null) deleteButton.interactable = false;
            }
            else
            {
                dataContainer.SetActive(true);
                emptyTextObject.SetActive(false);
                if (deleteButton != null) deleteButton.interactable = true;

                hpText.text = $"HP: {data.maxHp}";
                attackText.text = $"ATK: {data.maxAttack}";
                goldText.text = $"Gold: {data.gold:N0}";
                stoneText.text = $"Stone: {data.upgradeStones}";
                weaponLevel.text = $"weaponLevel: {data.weaponLevel}";
            }
        }

        private void OnLoadClicked()
        {
            if (DataManager.Instance.LoadGame(slotIndex))
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }

        private void OnSaveClicked()
        {
            DataManager.Instance.SaveGame(slotIndex);
            RefreshSlot();
        }

        private void OnDeleteClicked()
        {
            DataManager.Instance.DeleteSave(slotIndex);
            RefreshSlot();
        }
    }
}
