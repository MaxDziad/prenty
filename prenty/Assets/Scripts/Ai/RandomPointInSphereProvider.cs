using UnityEngine;

public class RandomPointInSphereProvider : AbstractPointProvider
{
	[SerializeField]
	private float _minRadius = 3f;

	[SerializeField]
	private float _maxRadius = 4f;

	public override Vector2 GetNextPoint(Vector2 origin)
	{
		var randomDirection = Random.insideUnitCircle.normalized;
		var randomDistance = Random.Range(_minRadius, _maxRadius);
		var point = origin + randomDirection * randomDistance;

		return point;
	}
}
