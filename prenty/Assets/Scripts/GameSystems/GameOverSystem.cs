using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameOverSystem : MonoBehaviour, IGameSystem, PlayerInputActionAsset.IEndScreenActions
{
	[SerializeField]
	private GameObject _resultScreen;

	[SerializeField]
	private TextMeshProUGUI _durationValue;

	[SerializeField]
	private TypeWriter _durationTypeWriter;

	[SerializeField]
	private TypeWriter _durationValueTypeWriter;

	[SerializeField]
	private TextMeshProUGUI _portalsClosedValue;

	[SerializeField]
	private TypeWriter _portalsTypeWriter;

	[SerializeField]
	private TypeWriter _portalsValueTypeWriter;

	[SerializeField]
	private TextMeshProUGUI _enemiesOnMapValue;

	[SerializeField]
	private TypeWriter _enemiesTypeWriter;

	[SerializeField]
	private TypeWriter _enemiesValueTypeWriter;

	[SerializeField]
	private TextMeshProUGUI _scoreValue;

	[SerializeField]
	private TypeWriter _scoreTypeWriter;

	[SerializeField]
	private TypeWriter _scoreValueTypeWriter;

	[SerializeField]
	private TypeWriter _exitGameTypeWriter;

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
		GetComponentsInChildren<TypeWriter>().ToList().ForEach(writer => writer.ClearText());
		StartCoroutine(EndScreenSequence());
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

	private IEnumerator EndScreenSequence()
	{
		yield return WritingSequence(_durationTypeWriter);
		yield return new WaitForSecondsRealtime(0.5f);
		yield return WritingSequence(_durationValueTypeWriter);
		yield return new WaitForSecondsRealtime(1f);
		yield return WritingSequence(_portalsTypeWriter);
		yield return new WaitForSecondsRealtime(0.5f);
		yield return WritingSequence(_portalsValueTypeWriter);
		yield return new WaitForSecondsRealtime(1f);
		yield return WritingSequence(_enemiesTypeWriter);
		yield return new WaitForSecondsRealtime(0.5f);
		yield return WritingSequence(_enemiesValueTypeWriter);
		yield return new WaitForSecondsRealtime(1f);
		yield return WritingSequence(_scoreTypeWriter);
		yield return new WaitForSecondsRealtime(0.5f);
		yield return WritingSequence(_scoreValueTypeWriter);
		yield return new WaitForSecondsRealtime(1f);
		yield return WritingSequence(_exitGameTypeWriter);
	}

	private IEnumerator WritingSequence(TypeWriter typeWriter)
	{
		typeWriter.StartTypewriter();

		while (typeWriter.IsTyping)
		{
			yield return null;
		}
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
