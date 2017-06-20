using UnityEngine;
using System.Collections;

public class Defense : MonoBehaviour {

    Animation animations;
    PlayerController playerCtrl;

    void Start()
    {
        animations = GetComponent<Animation>();
        playerCtrl = GetComponent<PlayerController>();
    }

    public void InDefenseState()
    {
        if (animations["DefenseAnimation"].speed == 1)
        {
            if (playerCtrl)
                playerCtrl.shieldRaised = true;
            animations.Stop();
        }
    }

    public void ShieldUp()
    {
        if (OtherIsPlaying() || playerCtrl.shieldRaised)
            return;

        animations["DefenseAnimation"].speed = 1;
        animations.Play("DefenseAnimation");
    }

    public void ShieldDown()
    {
        if (OtherIsPlaying())
            return;

        if (animations.IsPlaying("DefenseAnimation") || playerCtrl.shieldRaised)
        {
            if (playerCtrl.shieldRaised)
            {
                float length = animations.GetClip("DefenseAnimation").length;
                animations["DefenseAnimation"].time = length - 0.1f;
            }

            playerCtrl.shieldRaised = false;
            animations["DefenseAnimation"].speed = -1;
            animations.Play("DefenseAnimation");
        }
    }

    bool OtherIsPlaying()
    {
        if (animations.isPlaying && !animations.IsPlaying("DefenseAnimation"))
            return true;

        return false;
    }
}
