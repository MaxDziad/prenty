using UnityEngine;

public class PlayerPawn : MonoBehaviour, ISceneObject
{
	public void OnInitialize()
	{
		if (GameInstance.Instance.TryGetSystem(out PlayerPawnProviderSystem system))
		{
			system.RegisterPlayerPawn(this);
		}
	}
}
