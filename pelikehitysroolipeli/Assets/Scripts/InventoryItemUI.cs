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

        GetComponent<Button>().onClick.AddListener(UseItem);
    }

    void UseItem()
    {
        if (item.Use(player))
        {
            Debug.Log("Used " + item.itemName);
        }
    }
}