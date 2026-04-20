using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public TMP_Text itemText;

    Tavara item;
    PlayerController player;

    public void Setup(Tavara newItem, PlayerController p)
    {
        item = newItem;
        player = p;

        itemText.text = item.itemName;

        Button button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(UseItem);
    }

    void UseItem()
    {
        player.UseItem(item);
    }
}