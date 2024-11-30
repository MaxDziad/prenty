using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiAgentSpider : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private AbstractPointProvider _pointProvider;

	[SerializeField]
	private float _minimumDistanceToPathComplete = 0.1f;

	private NavMeshAgent _navmeshAgent;
	private bool _isInitialized;


	public void OnInitialize()
	{
		_navmeshAgent = GetComponent<NavMeshAgent>();
		_isInitialized = _navmeshAgent != null;
	}

	public void OnGameStart()
	{
		if (_isInitialized)
		{
			_navmeshAgent.updateRotation = false;
			_navmeshAgent.updateUpAxis = false;
			_navmeshAgent.enabled = true;
			SetNextPoint();
		}
	}

	public void Update()
	{
		if (_isInitialized)
		{
			transform.up = _navmeshAgent.velocity;
			CheckIfDestinationReached();
		}
	}

	private void CheckIfDestinationReached()
	{
		if (_navmeshAgent.remainingDistance <= _minimumDistanceToPathComplete)
		{
			SetNextPoint();
		}
	}

	private void SetNextPoint()
	{
		_navmeshAgent.SetDestination(_pointProvider.GetNextPoint(transform.position));
	}
}
