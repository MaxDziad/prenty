using System;
using Unity.Cinemachine;
using UnityEngine;

public class EffectDuringPortalDestruction : MonoBehaviour
{
	[SerializeField]
	private PortalBehaviour _portalBehaviour;

	[SerializeField]
	private CinemachineImpulseSource _impulseSource;

	[SerializeField]
	private float _maxAmplitude = 0.7f;

	[SerializeField]
	private Material _whiteNoiseMaterial;

	[SerializeField]
	[Range(0f, 1f)]
	private float _maxWhiteNoiseTransparency = 0.3f;

	public void Start()
	{
		_portalBehaviour.OnPortalDestructionProgressEvent += OnPortalDestructionProgress;
		_portalBehaviour.OnPortalDestructionEvent += OnPortalDestruction;
	}

	private void OnPortalDestructionProgress(float progress)
	{
		_whiteNoiseMaterial.SetFloat("_Transparency", progress * _maxWhiteNoiseTransparency);
		_impulseSource.GenerateImpulseWithForce(progress * _maxAmplitude);
	}

	private void OnPortalDestruction()
	{
		_whiteNoiseMaterial.SetFloat("_Transparency", 0);
		_portalBehaviour.OnPortalDestructionProgressEvent -= OnPortalDestructionProgress;
		_portalBehaviour.OnPortalDestructionEvent -= OnPortalDestruction;
	}

	private void OnDestroy()
	{
		if (_portalBehaviour != null)
		{
			_portalBehaviour.OnPortalDestructionProgressEvent -= OnPortalDestructionProgress;
			_portalBehaviour.OnPortalDestructionEvent -= OnPortalDestruction;
		}
	}
}
