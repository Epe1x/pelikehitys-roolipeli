using UnityEngine;

public class Nuoli : Tavara
{
    public ArrowType ArrowType;

    public GameObject arrowPrefab;
    public override bool Use(PlayerController player)
    {
        player.chosenArrow = this;
        Debug.Log("Selected arrow: " + itemName);
        return true;
    }
}
