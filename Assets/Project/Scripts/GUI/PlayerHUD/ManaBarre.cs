using UnityEngine;
using UnityEngine.UI;

public class ManaBarre : MonoBehaviour {

    private Slider manaBarre;

    void Awake()
    {
        manaBarre = GetComponent<Slider>();
    }

    public void Display(float ratio)
    {
        manaBarre.value = ratio;
    }
}
