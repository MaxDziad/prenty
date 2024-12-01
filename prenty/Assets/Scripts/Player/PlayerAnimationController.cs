using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	[SerializeField]
	private FlashlightController _flashlightController;

	[SerializeField]
	private Animator _playerBodyAnimator;

	[SerializeField]
	private Animator _playerHandAnimator;

	private readonly int _isMovingBool = Animator.StringToHash("IsMoving");
	private readonly int _isFlashlightOnBool = Animator.StringToHash("IsFlashlightOn");
	private readonly int _isDashingBool = Animator.StringToHash("IsDashing");

	public void Start()
	{
		_flashlightController.OnFlashlightChangedEvent += OnFlashlightChanged;
	}

	private void OnFlashlightChanged(bool isActive)
	{
		_playerHandAnimator.SetBool(_isFlashlightOnBool, isActive);
	}

	public void UpdateIsDashingInfo(bool isDashing)
	{
		_playerBodyAnimator.SetBool(_isDashingBool, isDashing);
	}

	public void UpdateMovementInfo(bool isMoving)
	{
		_playerBodyAnimator.SetBool(_isMovingBool, isMoving);
		_playerHandAnimator.SetBool(_isMovingBool, isMoving);
	}

	public void OnDestroy()
	{
		if (_flashlightController != null)
		{
			_flashlightController.OnFlashlightChangedEvent -= OnFlashlightChanged;
		}
	}
}
