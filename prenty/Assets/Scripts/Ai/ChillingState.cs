using UnityEngine;

public class ChillingState : AbstractAiState
{
	[SerializeField]
	private AbstractPointProvider _pointProvider;

	[SerializeField]
	private Material _material;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private float _minimumDistanceToPathComplete = 0.1f;

	[SerializeField]
	private float _speed = 1f;

	[SerializeField]
	private float _acceleration = 1f;

	public override void OnStart()
	{
		_spriteRenderer.material = _material;
		_agentSpider.Agent.speed = _speed;
		_agentSpider.Agent.acceleration = _acceleration;
		SetNextPoint();
	}

	public override void OnUpdate()
	{
		if (_agentSpider.Agent.velocity != Vector3.zero)
		{
			_agentSpider.transform.up = _agentSpider.Agent.velocity;
		}

		if (_agentSpider.Agent.remainingDistance <= _minimumDistanceToPathComplete)
		{
			SetNextPoint();
		}
	}

	private void SetNextPoint()
	{
		_agentSpider.Agent.SetDestination(_pointProvider.GetNextPoint(transform.position));
	}
}
