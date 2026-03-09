using UnityEngine;

public class Tavara : MonoBehaviour
{
    public string itemName;
    public float weight;
    public float volume;

    public virtual bool Use(PlayerController player)
    {
        Debug.Log(itemName + " can't be used");
        return false;
    }
}
