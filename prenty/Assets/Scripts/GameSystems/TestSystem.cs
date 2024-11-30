using UnityEngine;

public class TestSystem : MonoBehaviour, IGameSystem
{
	public void Initialize()
	{
		Debug.Log("System initialized");
	}

	public void OnSystemsInitialized()
	{
		Debug.Log("On Systems initialized");
	}

	public void OnSceneObjectsInitialized()
	{
		Debug.Log("On Scene Objects initialized");
	}

	public void OnSceneReady()
	{
		Debug.Log("On Scene Ready");
	}

	public void OnSceneStart()
	{
		Debug.Log("On Scene Start");
	}

	public void Uninitialize()
	{
		Debug.Log("Nara");
	}
}
