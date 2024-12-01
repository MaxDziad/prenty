using TMPro;
using UnityEngine;

public class GameOverSystem : MonoBehaviour, IGameSystem, PlayerInputActionAsset.IEndScreenActions
{
	[SerializeField]
	private GameObject _resultScreen;

	[SerializeField]
	private TextMeshProUGUI _durationValue;

	[SerializeField]
	private TextMeshProUGUI _portalsClosedValue;

	[SerializeField]
	private TextMeshProUGUI _enemiesOnMapValue;

	[SerializeField]
	private TextMeshProUGUI _scoreValue;

	private PlayerPawn _playerPawn;
	private InputSystem _inputSystem;
	private TimerSystem _timerSystem;
	private GameFlowSystem _gameFlowSystem;

	public void Initialize() { }

	public void OnSceneObjectsInitialized()
	{
		if (GameInstance.Instance.TryGetSystem<PlayerPawnProviderSystem>(out var system))
		{
			_playerPawn = system.PlayerPawn;
			_playerPawn.PlayerHealth.OnPlayerDeathEvent += OnPlayerDeath;
		}

		GameInstance.Instance.TryGetSystem(out _inputSystem);
		GameInstance.Instance.TryGetSystem(out _timerSystem);
		GameInstance.Instance.TryGetSystem(out _gameFlowSystem);

		_inputSystem.PlayerInputActions.EndScreen.AddCallbacks(this);
	}

	private void OnPlayerDeath()
	{
		Time.timeScale = 0;
		_inputSystem.PlayerInputActions.Gameplay.Disable();
		UpdateEndScreenInfo();
		_resultScreen.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void UpdateEndScreenInfo()
	{
		var timeSpan = _timerSystem.GetTimeSpan();
		_durationValue.text = timeSpan.Minutes + ":" + timeSpan.Seconds;
		_portalsClosedValue.text = _gameFlowSystem.DestroyedPortals.ToString();
		_enemiesOnMapValue.text = _gameFlowSystem.SpawnedSpiders.Count.ToString();
		_scoreValue.text = (_gameFlowSystem.DestroyedPortals * 100
			+ _gameFlowSystem.SpawnedSpiders.Count * 10
			+ timeSpan.Minutes).ToString();
	}

	public void Uninitialize()
	{
		_playerPawn = null;
		_inputSystem = null;
	}

	public void OnRestart(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (context.phase == UnityEngine.InputSystem.InputActionPhase.Performed
			&& _resultScreen.activeInHierarchy)
		{
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}
