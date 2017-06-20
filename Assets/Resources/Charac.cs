using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable/Characteristics", order = 1)]
public class Charac : ScriptableObject
{
    public int level;
    public int strength;
    public int constitution;
    public int intelligence;
    public int dexterity;
}
