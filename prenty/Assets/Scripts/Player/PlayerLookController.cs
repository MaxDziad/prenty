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
			var forward = _lookAtTarget.position - transform.position;
			transform.rotation = Quaternion.LookRotation(forward, Vector3.right);
		}
	}
}
