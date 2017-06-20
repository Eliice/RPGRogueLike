using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class SO_Weapon : SO_Equipable
{
    public int damage;
    public int range;

    override public string getMainStat()
    {
        return "Damage: " + damage;
    }
}
