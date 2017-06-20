using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float speed = 25f;
    private Space       space;
    private Vector3     rotatePoint;
    private Vector3     axis;
    private Transform   toRotate;
    private Vector3     toAngle;
    private bool localSpace = false;
    [HideInInspector] public bool inAnimation = false;

    public void Rotate(Transform t, Vector3 ax, Vector3 a, Vector3 r = default(Vector3), bool local = false)
    {
        if (local)
        {
            space = Space.Self;
            localSpace = true;
        }
        toRotate = t;
        rotatePoint = r;
        axis = ax;
        toAngle = a;
        inAnimation = true;
    }

    void Update()
    {
        if (!inAnimation)
            return;
        if (localSpace)
            toRotate.Rotate(axis * (speed * Time.deltaTime), space);
        else
            toRotate.RotateAround(rotatePoint, axis, speed * Time.deltaTime);
        if (toRotate.localEulerAngles.x >= toAngle.x && 
            toRotate.localEulerAngles.y >= toAngle.y &&
            toRotate.localEulerAngles.z >= toAngle.z)
        {
            inAnimation = false;
        }
    }
}