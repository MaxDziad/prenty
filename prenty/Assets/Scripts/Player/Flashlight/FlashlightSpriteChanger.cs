using UnityEngine;

public class FlashlightSpriteChanger : MonoBehaviour, ISceneObject
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite _flashlightOffSprite;

	[SerializeField]
	private Sprite _flashlightOnSprite;

	[SerializeField]
	private FlashlightController _flashlightController;

	public void OnInitialize()
	{
		_flashlightController.OnFlashlightChangedEvent += OnFlashlightChanged;
	}

	private void OnFlashlightChanged(bool active)
	{
		_spriteRenderer.sprite = active ? _flashlightOnSprite : _flashlightOffSprite;
	}

	private void OnDestroy()
	{
		if (_flashlightController != null)
		{
			_flashlightController.OnFlashlightChangedEvent -= OnFlashlightChanged;
		}
	}
}
