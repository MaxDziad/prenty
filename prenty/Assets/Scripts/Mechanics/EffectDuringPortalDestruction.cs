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
	private ParticleSystem _particleSystem;

	[SerializeField]
	[Range(0f, 1f)]
	private float _maxWhiteNoiseTransparency = 0.3f;

	private float _progress = 0;
	private float _newProgress=0;

	public void Start()
	{
		_portalBehaviour.OnPortalDestructionProgressEvent += OnPortalDestructionProgress;
		_portalBehaviour.OnPortalDestructionEvent += OnPortalDestruction;
	}

	public void Update()
	{
  //       Debug.Log("Stary: " + _progress + ", Nowy: " + _newProgress); 
		// if (_newProgress > _progress)
		// {
		// 	Debug.Log("Zaczynamy partikle");
		// 	_particleSystem.Play();
		// }
		// else
		// {
		// 	Debug.Log("Koniec partikle");
		// 	_particleSystem.Stop();
		// }
		// _progress = _newProgress;
	}

	private void OnPortalDestructionProgress(float progress)
	{
		_whiteNoiseMaterial.SetFloat("_Transparency", progress * _maxWhiteNoiseTransparency);
		_impulseSource.GenerateImpulseWithForce(progress * _maxAmplitude);
		if (progress > _newProgress)
		{
			_particleSystem.Play();
		}
		else if (progress <= _newProgress)
		{
			_particleSystem.Pause();
			_particleSystem.Clear();
		}

		_newProgress = progress;
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
