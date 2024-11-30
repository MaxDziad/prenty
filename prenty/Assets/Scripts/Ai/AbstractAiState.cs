using UnityEngine;

public class AbstractAiState : MonoBehaviour
{
	[SerializeField]
	protected SpiderState _spiderState;

	[SerializeField]
	protected AiAgentSpider _agentSpider;

	public SpiderState SpiderState => _spiderState;

	public virtual void OnStart() { }
	public virtual void OnUpdate() { }
	public virtual void OnFinish() { }
}
