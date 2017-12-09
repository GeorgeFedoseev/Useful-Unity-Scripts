using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;


[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
//[RequireComponent(typeof(RawImage))]
public class RawImageVideoPlayer : MonoBehaviour {

	RawImage rawImage;

	public string streamingAssetsRelativePath = "";
	private string oldStreamingAssetsRelativePath = "";

	[HideInInspector]
	public VideoPlayer videoPlayer;



	void Awake(){
		rawImage = GetComponent<RawImage> ();

		videoPlayer = GetComponent<VideoPlayer> ();
		videoPlayer.renderMode = VideoRenderMode.APIOnly;
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		videoPlayer.controlledAudioTrackCount = 1;
		//source.EnableAudioTrack (0, true);	
		videoPlayer.SetTargetAudioSource (0, GetComponent<AudioSource> ());	

	}

	// Use this for initialization
	void Start () {

	}



	// Update is called once per frame
	void Update () {

		// update vid path
		if(streamingAssetsRelativePath != oldStreamingAssetsRelativePath){
			InitVideo (streamingAssetsRelativePath);
			oldStreamingAssetsRelativePath = streamingAssetsRelativePath;
		}

	}

	// METHODS

	void InitVideo(string pathRelativeToStreamingAssets){

		var full_path = Path.Combine (Application.streamingAssetsPath, pathRelativeToStreamingAssets);

		if (File.Exists (full_path)) {
			videoPlayer.url = full_path;
			videoPlayer.Prepare ();

			videoPlayer.prepareCompleted += VideoPrepareCompleted;

		} else {
			Debug.LogError ("File not found: "+full_path);
			if (videoPlayer.isPrepared) {
				videoPlayer.Stop ();
			}
		}
	}


	// EVENTS
	bool audioFixFlag = false;
	void VideoPrepareCompleted(VideoPlayer source){
		//Debug.LogWarning ("VideoPrepareCompleted");

		rawImage.texture = videoPlayer.texture;



		var audioSource = GetComponent<AudioSource> ();
		if (audioSource != null) {
			Loom.QueueOnMainThread (() => {
				source.controlledAudioTrackCount = 1;
				//source.EnableAudioTrack (0, true);	

				Loom.QueueOnMainThread (() => {
					source.SetTargetAudioSource (0, audioSource);

					if (!audioFixFlag) {
						videoPlayer.Stop ();
						audioFixFlag = true;
						return;
					}
				});
			});
		}



	}
}
