using UnityEngine;

public class PlayerPawn : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private PlayerHealth _playerHealth;

	public PlayerHealth PlayerHealth => _playerHealth;

	public void OnInitialize()
	{
		if (GameInstance.Instance.TryGetSystem(out PlayerPawnProviderSystem system))
		{
			system.RegisterPlayerPawn(this);
		}
	}
}
