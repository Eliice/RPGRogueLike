using UnityEngine;
using UnityEngine.UI;

public class HpBarre : MonoBehaviour {

    private Slider hpBarre;

    void Awake()
    {
        hpBarre = GetComponent<Slider>();
    }

    public void Display(float ratio)
    {
        hpBarre.value = ratio;
    }
}
