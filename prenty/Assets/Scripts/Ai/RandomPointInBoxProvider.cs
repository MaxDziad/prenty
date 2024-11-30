using UnityEngine;

public class RandomPointInBoxProvider : AbstractPointProvider
{
	[SerializeField]
	private Vector2 _xRange = new Vector2(-9, 9);

	[SerializeField]
	private Vector2 _yRange = new Vector2(-9, 9);

	public override Vector2 GetNextPoint(Vector2 origin)
	{
		float x = Random.Range(_xRange.x, _xRange.y);
		float y = Random.Range(_yRange.x, _yRange.y);
		return new Vector2(x, y);
	}
}
