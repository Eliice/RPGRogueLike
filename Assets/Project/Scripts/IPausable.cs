using UnityEngine;

public interface IPausable
{
	bool IsPaused { get; set; }

	void Pause();
	void Unpause();
	void TogglePause();
}