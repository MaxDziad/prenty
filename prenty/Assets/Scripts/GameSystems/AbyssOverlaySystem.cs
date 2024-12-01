using UnityEngine;

public class AbyssOverlaySystem : MonoBehaviour, IGameSystem
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private float _maxTimer = 20f;

	private readonly int _timeFloat = Animator.StringToHash("Time");

	private bool _isReady;
	private float _currentTime;

	public void Initialize() { }

	public void AddToTimer(float time)
	{
		_currentTime = Mathf.Clamp(0, _currentTime + time, _maxTimer);
	}

	public void OnSceneStart()
	{
		_isReady = true;
	}

	private void LateUpdate()
	{
		if (_isReady)
		{
			_currentTime += Time.deltaTime;
			_animator.SetFloat(_timeFloat, _currentTime / _maxTimer);
		}
	}

	public void Uninitialize()
	{
		_isReady = false;
	}
}
