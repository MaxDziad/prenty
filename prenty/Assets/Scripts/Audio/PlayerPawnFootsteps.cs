using UnityEngine;

public class PlayerPawnFootsteps : MonoBehaviour
{
	[SerializeField]
	private AudioSource _footstepSource1;

	[SerializeField]
	private AudioSource _footstepSource2;

	[SerializeField]
	private AudioSource _footstepSource3;

	public void PlayStep1()
	{
		_footstepSource1.Play();
	}

	public void PlayStep2()
	{
		_footstepSource2.Play();
	}

	public void PlayStep3()
	{
		_footstepSource3.Play();
	}
}
