using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Armor", menuName = "Armor", order = 2)]
public class SO_Armor : SO_Equipable
{
    public int armor;

    override public string getMainStat()
    {
        return "Armor: " + armor;
    }
}
