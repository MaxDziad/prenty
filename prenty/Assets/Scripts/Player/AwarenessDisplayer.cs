using System;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessDisplayer : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private GameObject _markerPrefab;

	[SerializeField]
	private Transform _markersHolder;

	[SerializeField]
	private float _distanceFromCenter;

	private AwarenessSystem _awarenessSystem;
	private readonly Dictionary<AwareObject, GameObject> _markers = new();

	public void OnInitialize() { }

	public void OnSceneSystemsPrepared()
	{
		if (GameInstance.Instance.TryGetSystem(out _awarenessSystem))
		{
			_awarenessSystem.OnAwareObjectAddedEvent += OnAwareObjectAdded;
			_awarenessSystem.OnAwareObjectRemovedEvent += OnAwareObjectRemoved;
		}
	}

	private void OnAwareObjectAdded(AwareObject obj)
	{
		if (!_markers.ContainsKey(obj))
		{
			var marker = Instantiate(_markerPrefab, _markersHolder);
			_markers.Add(obj, marker);
		}
	}

	private void OnAwareObjectRemoved(AwareObject obj)
	{
		if (_markers.TryGetValue(obj, out var marker))
		{
			_markers.Remove(obj);
			Destroy(marker);
		}
	}

	public void LateUpdate()
	{
		foreach (var keyValuePair in _markers)
		{
			UpdateMarkerPosition(keyValuePair.Value, keyValuePair.Key.ObjectPosition);
		}
	}

	private void UpdateMarkerPosition(GameObject marker, Vector3 objectPosition)
	{
		Vector3 direction = objectPosition - marker.transform.position;
		direction.Normalize();
		marker.transform.position = transform.position + direction * _distanceFromCenter;
		marker.transform.up = direction;
	}

	private void OnDestroy()
	{
		if (_awarenessSystem != null)
		{
			_awarenessSystem.OnAwareObjectAddedEvent -= OnAwareObjectAdded;
			_awarenessSystem.OnAwareObjectRemovedEvent -= OnAwareObjectRemoved;
		}

		_markers.Clear();
	}
}
