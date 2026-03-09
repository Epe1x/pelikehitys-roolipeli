using UnityEngine;

public class Jousi : Tavara
{
    public int range = 5;

    public override bool Use(PlayerController player)
    {
        player.chosenWeapon = this;
        Debug.Log("Bow equipped");
        return true;
    }
}
