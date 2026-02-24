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
        [SerializeField] private Animation blockAnimation;

        [SerializeField] private List<slotData> itemSprites;

        private int itemId;
        public string ItemName => itemSprites[itemId].name;

        private void Awake()
        {
            ResetData();
        }

        public void ResetData()
        {
            itemId = Random.Range(0, itemSprites.Count - 1);

            if (itemText != null)
                itemText.text = itemSprites[itemId].name;

            itemImage.sprite = itemSprites[itemId].sprite;
        }

        public void Blink()
        {
            if (blockAnimation)
                blockAnimation.Play();
        }
    }
}