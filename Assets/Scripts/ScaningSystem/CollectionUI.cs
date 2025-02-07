using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class CollectionUI : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Button changePageButton;
    [SerializeField] private TMP_Text pageTitleText;
    private RarityGrade currentRarity = RarityGrade.Trash;
    private void OnEnable()
    {
        CollectionManager.Instance.OnItemCollected += RefreshUI;
        changePageButton.onClick.AddListener(ChangePage);
        RefreshUI();
    }
    private void OnDisable()
    {
        if (CollectionManager.Instance != null)
            CollectionManager.Instance.OnItemCollected -= RefreshUI;
        changePageButton.onClick.RemoveListener(ChangePage);
    }
    public void RefreshUI(CollectibleItem collectedItem = null)
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);
        List<CollectibleItem> filteredItems = CollectionManager.Instance.allItems
            .Where(item => item.rarity == currentRarity)
            .ToList();
        foreach (var item in filteredItems)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            CollectionSlot slot = slotObj.GetComponent<CollectionSlot>();
            if (slot != null)
                slot.SetItem(item);
        }
        pageTitleText.text = currentRarity.ToString();
    }
    private void ChangePage()
    {
        currentRarity = GetNextRarity(currentRarity);
        RefreshUI();
    }
    private RarityGrade GetNextRarity(RarityGrade current)
    {
        switch (current)
        {
            case RarityGrade.Trash: return RarityGrade.Common;
            case RarityGrade.Common: return RarityGrade.Green;
            case RarityGrade.Green: return RarityGrade.Blue;
            case RarityGrade.Blue: return RarityGrade.Epic;
            case RarityGrade.Epic: return RarityGrade.Legendary;
            case RarityGrade.Legendary: return RarityGrade.Trash;
            default: return RarityGrade.Common;
        }
    }
}
