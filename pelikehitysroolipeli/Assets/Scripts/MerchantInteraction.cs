using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MerchantInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject merchantPanel;
    public TMP_Text nameLabel;
    public TMP_Dropdown choiceDropdown;

    public Button buyButton;
    public Button cancelButton;

    private Merchant currentMerchant;

    public GameObject eliittiNuoliPrefab;
    public GameObject perusNuoliPrefab;
    public GameObject aloittelijaNuoliPrefab;

    void Start()
    {
        merchantPanel.SetActive(false);

        buyButton.onClick.AddListener(BuyItem);
        cancelButton.onClick.AddListener(CloseMerchantUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        Merchant merchant = other.GetComponentInParent<Merchant>();

        if (merchant != null)
        {
            currentMerchant = merchant;
            OpenMerchantUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Merchant merchant = other.GetComponentInParent<Merchant>();

        if (merchant != null && merchant == currentMerchant)
        {
            CloseMerchantUI();
        }
    }

    public void OpenMerchantUI()
    {
        merchantPanel.SetActive(true);
        nameLabel.text = currentMerchant.merchantName;

        choiceDropdown.ClearOptions();
        List<string> options;

        if (currentMerchant.merchantType == MerchantType.Arrow)
            options = new List<string>(Enum.GetNames(typeof(ArrowType)));
        else
            options = new List<string>(Enum.GetNames(typeof(FoodType)));

        choiceDropdown.AddOptions(options);
    }

    public void CloseMerchantUI()
    {
        merchantPanel.SetActive(false);
        currentMerchant = null;
    }

    void BuyItem()
    {
        if (currentMerchant == null)
            return;

        int selectedIndex = choiceDropdown.value;

        if (currentMerchant.merchantType == MerchantType.Arrow)
        {
            ArrowType selectedArrow = (ArrowType)selectedIndex;
            BuyArrowFromDropdown();
        }
        else
        {
            FoodType selectedFood = (FoodType)selectedIndex;
            BuyFoodFromDropdown();
        }
    }

    public void BuyArrowFromDropdown()
    {
        int selectedIndex = choiceDropdown.value;
        ArrowType arrow = (ArrowType)selectedIndex;

        int cost = ArrowDatabase.GetArrowPrice(arrow);

        if (!PlayerDataManager.Instance.TakeCoins(cost))
        {
            Debug.Log("Not enough coins");
            return;
        }

        GameObject prefabToSpawn = null;

        switch (arrow)
        {
            case ArrowType.Eliittinuoli:
                prefabToSpawn = eliittiNuoliPrefab;
                break;

            case ArrowType.Aloittelijanuoli:
                prefabToSpawn = aloittelijaNuoliPrefab;
                break;

            case ArrowType.Perusnuoli:
                prefabToSpawn = perusNuoliPrefab;
                break;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("Arrow prefab is missing");
            return;
        }

        PlayerController player = FindFirstObjectByType<PlayerController>();

        GameObject arrowObj = Instantiate(prefabToSpawn);
        arrowObj.SetActive(false);

        Nuoli arrowItem = arrowObj.GetComponent<Nuoli>();

        bool added = player.playerReppu.AddItem(arrowItem);

        if (added)
        {
            player.UpdateInventoryUI();
            Debug.Log("Arrow added to inventory: " + arrowItem.itemName);
        }
        else
        {
            Debug.Log("Inventory full");
            Destroy(arrowObj);
        }
        Debug.Log("Inventory count: " + player.playerReppu.GetItems().Count);
    }

    public void BuyFoodFromDropdown()
    {
        int selectedIndex = choiceDropdown.value;

        FoodType food = (FoodType)selectedIndex;

        int cost = FoodDatabase.GetFoodPrice(food);
        int health = FoodDatabase.GetFoodHeal(food);

        if (PlayerDataManager.Instance.TakeCoins(cost))
        {
            PlayerDataManager.Instance.AddHealth(health);
            Debug.Log("Bought food: " + food);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void SetCurrentMerchant(Merchant merchant)
    {
        currentMerchant = merchant;
    }
}