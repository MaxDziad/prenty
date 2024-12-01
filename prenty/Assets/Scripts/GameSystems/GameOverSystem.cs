using UnityEngine;

public class GameOverSystem : MonoBehaviour, IGameSystem
{
	private PlayerPawn _playerPawn;
	private InputSystem _inputSystem;

	public void Initialize() { }

	public void OnSceneObjectsInitialized()
	{
		if (GameInstance.Instance.TryGetSystem<PlayerPawnProviderSystem>(out var system))
		{
			_playerPawn = system.PlayerPawn;
			_playerPawn.PlayerHealth.OnPlayerDeathEvent += OnPlayerDeath;
		}

		GameInstance.Instance.TryGetSystem(out _inputSystem);
	}

	private void OnPlayerDeath()
	{
		Time.timeScale = 0;
		_inputSystem.PlayerInputActions.Gameplay.Disable();
	}

	public void Uninitialize()
	{
		_playerPawn = null;
		_inputSystem = null;
	}
}
