using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowSystem : MonoBehaviour, IGameSystem
{
	[Header("Spider Settings")]
	[SerializeField]
	private AiAgentSpider _spiderPrefab;

	[SerializeField]
	private Transform _spiderHolder;

	[SerializeField]
	private AbstractPointProvider _randomSpiderSpawnPointProvider;

	[SerializeField]
	private float _minimumDistanceFromPlayer = 5;

	[SerializeField]
	private int _maximumSpiders = 20;

	[Header("Portal Settings")]

	[SerializeField]
	private PortalBehaviour _portalPrefab;

	[SerializeField]
	private Transform _portalSpawnPointsHolder;

	[SerializeField]
	private float _timeForNewPortalSpawn = 4f;

	public int DestroyedPortals { get; private set; } = 0;
	public List<AiAgentSpider> SpawnedSpiders => _spawnedSpiders;

	private readonly List<AiAgentSpider> _spawnedSpiders = new();

	private List<Transform> _portalSpawnPoints;
	private PortalBehaviour _currentPortal;
	private PlayerPawn _playerPawn;
	private AbyssOverlaySystem _overlaySystem;

	private Coroutine _waitHandle;
	private Coroutine _brainDisableHandle;

	public void Initialize()
	{
		_spawnedSpiders.Capacity = 20;
		_portalSpawnPoints = new(_portalSpawnPointsHolder.GetComponentsInChildren<Transform>());
	}

	public void OnSystemsInitialized()
	{
		GameInstance.Instance.TryGetSystem(out _overlaySystem);
	}

	public void OnSceneObjectsInitialized()
	{
		if (GameInstance.Instance.TryGetSystem<PlayerPawnProviderSystem>(out var system))
		{
			_playerPawn = system.PlayerPawn;
			_playerPawn.PlayerHealth.OnPlayerTakeDamageEvent += OnPlayerTakeDamage;
			_playerPawn.PlayerHealth.OnPlayerDeathEvent += OnPlayerDeath;
		}
	}

	private void OnPlayerTakeDamage()
	{
		_brainDisableHandle = StartCoroutine(TemporaryAiBrainDisable());
	}

	private IEnumerator TemporaryAiBrainDisable()
	{
		_spawnedSpiders.ForEach(spider => spider.SetBrainActive(false));
		yield return new WaitForSeconds(3f);
		_spawnedSpiders.ForEach(spider => spider.SetBrainActive(true));
	}

	public void OnSceneStart()
	{
		SpawnNewPortal();
	}

	private void SpawnNewPortal()
	{
		_currentPortal = Instantiate(_portalPrefab, GetRandomPortalPoint(), Quaternion.identity);
		_currentPortal.OnPortalDestructionEvent += OnPortalDestruction;
	}

	private void OnPortalDestruction()
	{
		_currentPortal.OnPortalDestructionEvent -= OnPortalDestruction;
		_waitHandle = StartCoroutine(WaitForNewPortalSpawn());
		DestroyedPortals++;

		if (_overlaySystem != null)
		{
			_overlaySystem.AddToTimer(-15);
		}
	}

	private IEnumerator WaitForNewPortalSpawn()
	{
		yield return new WaitForSeconds(_timeForNewPortalSpawn);
		SpawnNewPortal();
		yield return null;
		TrySpawnNewEnemy();
		yield return null;
		TrySpawnNewEnemy();
	}

	private void TrySpawnNewEnemy()
	{
		if (_spawnedSpiders.Count < _maximumSpiders)
		{
			SpawnNewEnemy();
		}
	}

	private void SpawnNewEnemy()
	{
		var pawnPosition = _playerPawn.transform.position;
		var point = _playerPawn.transform.position;

		while (Vector2.Distance(point, pawnPosition) < _minimumDistanceFromPlayer)
		{
			point = _randomSpiderSpawnPointProvider.GetNextPoint(Vector2.zero);
		}

		var spider = Instantiate(_spiderPrefab, _spiderHolder);
		spider.transform.position = point;
		_spawnedSpiders.Add(spider);
		spider.OnSpiderDestroyEvent += OnSpiderDestroy;
		spider.Spawn();
	}

	private void OnSpiderDestroy(AiAgentSpider spider)
	{
		spider.OnSpiderDestroyEvent -= OnSpiderDestroy;

		if (_spawnedSpiders.Contains(spider))
		{
			_spawnedSpiders.Remove(spider);
		}
	}

	private Vector3 GetRandomPortalPoint()
	{
		var pawnPosition = _playerPawn.transform.position;
		var point = _playerPawn.transform.position;

		while (Vector2.Distance(point, pawnPosition) < _minimumDistanceFromPlayer)
		{
			point = _portalSpawnPoints[Random.Range(0, _portalSpawnPoints.Count)].position;
		}

		return point;
	}

	private void OnPlayerDeath()
	{
		_playerPawn.PlayerHealth.OnPlayerDeathEvent -= OnPlayerDeath;
		GameOver();
	}

	private void GameOver()
	{
		if (_waitHandle != null)
		{
			StopCoroutine(_waitHandle);
		}

		if (_brainDisableHandle != null)
		{
			StopCoroutine(_brainDisableHandle);
		}
	}

	public void Uninitialize()
	{
		if (_waitHandle != null)
		{
			StopCoroutine(_waitHandle);
		}

		if (_brainDisableHandle != null)
		{
			StopCoroutine(_brainDisableHandle);
		}

		_playerPawn = null;
		_spawnedSpiders.Clear();
	}
}
