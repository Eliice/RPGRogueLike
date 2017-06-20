using UnityEngine;
using UnityEngine.UI;

public class PexBarre : MonoBehaviour {


    private Slider pexBarre;

    void Awake()
    {
        pexBarre = GetComponent<Slider>();
    }

    public void Display(int value)
    {
        pexBarre.value = value;
    }
}