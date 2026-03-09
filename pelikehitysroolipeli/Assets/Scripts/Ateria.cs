using UnityEngine;

public class Ateria : Tavara
{
    public int healAmount = 15;

    public override bool Use(PlayerController player)
    {
        PlayerDataManager.Instance.AddHealth(healAmount);
        Debug.Log("Health increase by " + healAmount);
        return true;
    }
}
