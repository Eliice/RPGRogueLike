using UnityEngine;
using System.Collections;

public enum Slot
{
    HEAD,
    CHEST,
    LEGS,
    BOOT,
    RIGHT_HANDED,
    LEFT_HANDED
}

public class SO_Equipable : SO_Item
{
    public Slot slot;

    public override string getSlot()
    {
        return slot.ToString();
    }
}
