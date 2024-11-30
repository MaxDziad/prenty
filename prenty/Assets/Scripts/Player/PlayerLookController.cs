using UnityEngine;

public class PlayerLookController : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private Transform _lookAtTarget;

	private bool _isInitialized;

	public void OnInitialize()
	{
		_isInitialized = true;
	}

	private void Update()
	{
		if (_isInitialized)
		{
			transform.up = (_lookAtTarget.position - transform.position).normalized;
		}
	}
}
