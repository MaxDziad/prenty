using UnityEngine;

public class PortalSpriteUpdater : MonoBehaviour
{
	[SerializeField]
	private PortalBehaviour _portalBehaviour;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Sprite _noneSprite;

	[SerializeField]
	private Sprite _damagedSprite;

	[SerializeField]
	private Sprite _almostDestroyedSprite;

	// Anim event function
	public void Destroy()
	{
		_portalBehaviour.Destroy();
	}

	private void Start()
	{
		_portalBehaviour.OnPortalStageChangedEvent += OnPortalStageChanged;
		_portalBehaviour.OnPortalDestructionEvent += OnPortalDestruction;
	}

	private void OnPortalStageChanged(PortalDestructionStage stage)
	{
		var sprite = stage == PortalDestructionStage.None ? _noneSprite
			: stage == PortalDestructionStage.Damaged ? _damagedSprite
			: _almostDestroyedSprite;

		_spriteRenderer.sprite = sprite;
	}

	private void OnPortalDestruction()
	{
		_animator.enabled = true;
	}

	private void OnDestroy()
	{
		if (_portalBehaviour != null)
		{
			_portalBehaviour.OnPortalStageChangedEvent -= OnPortalStageChanged;
			_portalBehaviour.OnPortalDestructionEvent -= OnPortalDestruction;
		}
	}
}
