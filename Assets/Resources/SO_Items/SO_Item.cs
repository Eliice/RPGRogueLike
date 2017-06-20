using UnityEngine;
using System.Collections;

public abstract class SO_Item : ScriptableObject
{
    new public string name;
    public int weight;
    public int value;
    public bool isDisposable;
    public bool isQuestItem;
    public GameObject prefab;

    virtual public string getSlot()
    {
        return "";
    }

    virtual public string getMainStat()
    {
        return "";
    }
}
