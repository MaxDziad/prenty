using System;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessSystem : MonoBehaviour, IGameSystem
{
	public event Action<AwareObject> OnAwareObjectAddedEvent;
	public event Action<AwareObject> OnAwareObjectRemovedEvent;

	public readonly List<AwareObject> RegisteredObjects = new();

	public void Initialize()
	{
		RegisteredObjects.Capacity = 30;
	}

	public void RegisterAwareObject(AwareObject obj)
	{
		if (!RegisteredObjects.Contains(obj))
		{
			RegisteredObjects.Add(obj);
			OnAwareObjectAddedEvent?.Invoke(obj);
		}
	}

	public void DeregisterAwareObject(AwareObject obj)
	{
		if (RegisteredObjects.Contains(obj))
		{
			RegisteredObjects.Remove(obj);
			OnAwareObjectRemovedEvent?.Invoke(obj);
		}
	}

	public void Uninitialize()
	{
		RegisteredObjects.ForEach(obj => OnAwareObjectRemovedEvent?.Invoke(obj));
		RegisteredObjects.Clear();
	}
}
