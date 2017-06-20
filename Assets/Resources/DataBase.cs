using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataBase", menuName = "DataBase")]
public class DataBase : ScriptableObject
{
    public List<ScriptableObject> dataBase = new List<ScriptableObject>();
}
