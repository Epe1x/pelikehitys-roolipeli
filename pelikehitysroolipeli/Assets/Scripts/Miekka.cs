using UnityEngine;

public class Miekka : Tavara
{
    public int damage = 10;

    public override bool Use(PlayerController player)
    {
        player.chosenWeapon = this;
        Debug.Log("Sword chosen");
        return true;
    }
}
