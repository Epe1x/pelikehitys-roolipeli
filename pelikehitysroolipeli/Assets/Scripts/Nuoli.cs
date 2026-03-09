using UnityEngine;

public class Nuoli : Tavara
{
    public int damage = 5;
    public ArrowType ArrowType;

    public override bool Use(PlayerController player)
    {
        player.chosenArrow = this;
        Debug.Log("Selected arrow: " + itemName);
        return true;
    }
}
