using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
	private List<IGameSystem> allSystems;
	private List<ISceneObject> sceneObjects;

	public static GameInstance Instance;
	public bool TryGetSystem<TType>(out TType system)
	{
		system = GetComponentInChildren<TType>();
		return system != null;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		InitializeGame();
	}

	private void InitializeGame()
	{
		GatherAllSystems();
		GatherAllSceneObjects();
		PrepareAndStartGame();
	}

	private void GatherAllSystems()
	{
		allSystems = new(GetComponentsInChildren<IGameSystem>(true));
	}

	private void GatherAllSceneObjects()
	{
		sceneObjects = new(FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISceneObject>());
	}

	private void PrepareAndStartGame()
	{
		allSystems.ForEach(s => s.Initialize());
		allSystems.ForEach(s => s.OnSystemsInitialized());
		sceneObjects.ForEach(o => o.OnInitialize());
		allSystems.ForEach(s => s.OnSceneObjectsInitialized());
		sceneObjects.ForEach(o => o.OnSceneSystemsPrepared());
		allSystems.ForEach(s => s.OnSceneReady());
		sceneObjects.ForEach(o => o.OnGameStart());
		allSystems.ForEach(s => s.OnSceneStart());
	}

	private void OnDestroy()
	{
		foreach (var s in allSystems)
		{
			if (s != null)
			{
				s.Uninitialize();
			}
		}

		allSystems.Clear();
		sceneObjects.Clear();
	}
}
