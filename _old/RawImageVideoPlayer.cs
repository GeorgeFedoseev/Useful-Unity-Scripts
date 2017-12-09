using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;


[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(RawImage))]
public class RawImageVideoPlayer : MonoBehaviour {


	public string streamingAssetsRelativePath = "";
	private string oldStreamingAssetsRelativePath = "";

	VideoPlayer videoPlayer;
	RawImage rawImage;


	void Awake(){
		rawImage = GetComponent<RawImage> ();

		videoPlayer = GetComponent<VideoPlayer> ();
		videoPlayer.renderMode = VideoRenderMode.APIOnly;
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

	void VideoPrepareCompleted(VideoPlayer source){
		Debug.LogWarning ("VideoPrepareCompleted");

		rawImage.texture = videoPlayer.texture;
	}
}
