using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TASK3.SlotMachine
{
    public class SlotItem : MonoBehaviour
    {
        [System.Serializable]
        public class slotData
        {
            public string name;
            public Sprite sprite;
        }
        [SerializeField] private UnityEngine.UI.Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;
        [SerializeField] private Color[] rarityColors;

        [SerializeField] private List<slotData> itemSprites;

        private int itemId;
        public string ItemName => itemSprites[itemId].name;

        public void SetItemData(string name)
        {
            itemId = Random.Range(0, itemSprites.Count - 1);

            if (itemText != null)
                itemText.text = name;

            if (itemImage != null && rarityColors.Length > 0)
            {
                // Устанавливаем цвет в зависимости от редкости
                int colorIndex = Mathf.Clamp(itemId, 0, rarityColors.Length - 1);
                itemImage.color = rarityColors[colorIndex];
                itemImage.sprite = itemSprites[itemId].sprite;
            }
        }

        public int GetItemId()
        {
            return itemId;
        }
    }
}