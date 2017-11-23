using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController {
    private string videoAbsPath;//where the video lives

    private VideoPlayer video;//the canvas the video will be displayed
    private GameObject plane;

    public VideoController(VideoPlayer video)
    {
        this.video = video;
        this.plane = GameObject.Find("VideoPlane");
      
    }

    public void play()
    {
        this.video.Play();
    }

    public void pause()
    {
        this.video.Pause();
    }

    public void restart()
    {
        this.video.frame = 0;
    }

    public void setEnabled(bool set)
    {
        this.video.enabled = set;
        plane.SetActive(set);
    }

    public void toggleEnabled()
    {
        this.video.enabled = !this.video.enabled;
        plane.SetActive(video.enabled);

    }
}

