using UnityEngine;

public class AwareObject : MonoBehaviour
{
	[SerializeField]
	private Transform _transform;

	private AwarenessSystem _awarenessSystem;

	public Vector3 ObjectPosition => _transform.position;

	public void Start()
	{
		if (GameInstance.Instance.TryGetSystem(out _awarenessSystem))
		{
			_awarenessSystem.RegisterAwareObject(this);
		}
	}

	private void OnDisable()
	{
		if (_awarenessSystem != null)
		{
			_awarenessSystem.DeregisterAwareObject(this);
		}
	}

	private void OnDestroy()
	{
		if (_awarenessSystem != null)
		{
			_awarenessSystem.DeregisterAwareObject(this);
		}
	}
}
