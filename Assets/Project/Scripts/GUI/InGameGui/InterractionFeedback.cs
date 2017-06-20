using UnityEngine;
using UnityEngine.UI;

public class InterractionFeedback : Menu
{
	[SerializeField] private Text descriptionLabel = null;

#if UNITY_EDITOR
	protected override void Awake()
	{
		base.Awake();

		if (descriptionLabel == null) Debug.LogError("descriptionLabel == null");
	}
#endif

	public override void OnShow(params object[] parameters)
	{
		descriptionLabel.text = parameters[0] as string;
	}
}