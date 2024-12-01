using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayingSystem : MonoBehaviour
{
	[SerializeField]
	private VideoPlayer _videoPlayer;

	public async Task PlayVideo()
	{
		gameObject.SetActive(true);
		_videoPlayer.Prepare();

		while (!_videoPlayer.isPrepared)
		{
			await Task.Yield();
		}

		_videoPlayer.Play();

		while (_videoPlayer.isPlaying)
		{
			await Task.Delay(50);
		}

		_videoPlayer.Stop();
		gameObject.SetActive(false);
	}
}
