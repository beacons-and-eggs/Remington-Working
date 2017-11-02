using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController {
    private string videoAbsPath;//where the video lives

    private VideoPlayer vid;//the canvas the video will be displayed
    private GameObject plane;

    public VideoController(string videoAbsPath, GameObject screen)
    {
        this.videoAbsPath = videoAbsPath;
        this.plane = GameObject.Find("VideoPlane");
        this.vid = screen.GetComponentInChildren<VideoPlayer>();
    }

    public void play()
    {
        this.vid.Play();
    }

    public void pause()
    {
        this.vid.Pause();
    }

    public void restart()
    {
        this.vid.frame = 0;
    }

    public void setEnabled(bool set)
    {
        this.vid.enabled = set;
        plane.SetActive(set);
    }

    public void toggleEnabled()
    {
        this.vid.enabled = !this.vid.enabled;
        plane.SetActive(vid.enabled);

    }
}

