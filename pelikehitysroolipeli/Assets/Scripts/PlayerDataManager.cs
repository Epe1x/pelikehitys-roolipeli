using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Player Values")]
    public int experience = 0;
    public int money = 0;
    public int health = 100;

    [Header("UI Elements")]
    public TMP_Text experienceText;
    public TMP_Text coinText;
    public TMP_Text hitpointText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    public void AddExperience(int amount)
    {
        experience += Mathf.Max(0, amount);
        UpdateUI();
    }

    public void AddHealth(int amount)
    {
        health += Mathf.Max(0, amount);
        UpdateUI();
    }

    public int RemoveHealth(int damageAmount)
    {
        health -= Mathf.Max(0, damageAmount);
        health = Mathf.Max(0, health);

        UpdateUI();
        return health;
    }

    public int AddCoins(int coinAmount)
    {
        money += Mathf.Max(0, coinAmount);
        UpdateUI();
        return money;
    }

    public bool TakeCoins(int coinAmount)
    {
        if (money < coinAmount)
            return false;

        money -= coinAmount;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (experienceText != null)
            experienceText.text = "XP: " + experience;

        if (coinText != null)
            coinText.text = "Coins: " + money;

        if (hitpointText != null)
            hitpointText.text = "HP: " + health;
    }

    void OnGUI()
    {
        int x = 10;
        int y = 200;
        int w = 150;
        int h = 30;

        if (GUI.Button(new Rect(x, y, w, h), "Add XP"))
            AddExperience(10);

        if (GUI.Button(new Rect(x, y += 35, w, h), "Take damage"))
            RemoveHealth(10);

        if (GUI.Button(new Rect(x, y += 35, w, h), "Heal"))
            AddHealth(10);

        if (GUI.Button(new Rect(x, y += 35, w, h), "Add coins"))
            AddCoins(5);

        if (GUI.Button(new Rect(x, y += 35, w, h), "Spend coins"))
            TakeCoins(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
