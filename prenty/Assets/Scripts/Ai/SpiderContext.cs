using UnityEngine;

public class SpiderContext : MonoBehaviour
{
	[SerializeField]
	private AiAgentSpider _spiderAgent;

	public AiAgentSpider SpiderAgent => _spiderAgent;
}
