using UnityEngine;

public class Vesi : Tavara
{
    public int healAmount = 10;

    public override bool Use(PlayerController player)
    {
        PlayerDataManager.Instance.AddHealth(healAmount);
        Debug.Log("Health increased by " + healAmount);
        return true;
    }
}
