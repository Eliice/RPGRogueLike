using UnityEngine;

public class DoorBehaviour : MonoBehaviour, IInterract
{
    private RotateObject rotateObject;
    private Vector3 rotatePoint;
    private bool doorOpen = false;

    void Start()
    {
        rotatePoint = transform.parent.position;
        rotateObject = GetComponent<RotateObject>();
    }

    public void Use()
    {
        if (rotateObject.inAnimation)
            return;
        if (!doorOpen)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            rotateObject.Rotate(transform, new Vector3(0, 1, 0), new Vector3(0, 90, 0), rotatePoint);
            doorOpen = true;
        }
        else
        {
            rotateObject.Rotate(transform, new Vector3(0, -1, 0), new Vector3(0, 355, 0), rotatePoint);
            doorOpen = false;
        }
    }

    public bool CanInterract()
    {
        return !rotateObject.inAnimation;
	}
	public string InterractionDescription()
	{
		return "[E] : Open";
	}
}