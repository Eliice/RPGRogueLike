using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MagicBase", order = 2)]
public class MagicBase : ScriptableObject
{
    
    public string spellName;
    public int manaCost;
    public float intelRatio;
    public Magic.Type type;
    public int speed;
}

