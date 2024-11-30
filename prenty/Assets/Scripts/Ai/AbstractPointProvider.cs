using UnityEngine;

public abstract class AbstractPointProvider : MonoBehaviour
{
	public abstract Vector2 GetNextPoint(Vector2 origin);
}
