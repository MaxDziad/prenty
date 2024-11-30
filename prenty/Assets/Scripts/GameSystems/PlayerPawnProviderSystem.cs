using UnityEngine;

public class PlayerPawnProviderSystem : MonoBehaviour, IGameSystem
{
	public PlayerPawn PlayerPawn { get; private set; }

	public void RegisterPlayerPawn(PlayerPawn playerPawn)
	{
		PlayerPawn = playerPawn;
	}

	public void Initialize() { }

	public void Uninitialize()
	{
		PlayerPawn = null;
	}
}
