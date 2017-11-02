using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerController {

    private SlideController sController;
    private VideoController vController;
    private enum ActiveController { slide, video };
    private ActiveController aController;


    public ViewerController(string slideAbsPath, string videoAbsPath, GameObject screen)
    {
        this.aController = ActiveController.slide;

        this.sController = new SlideController(slideAbsPath, screen);
        this.vController = new VideoController(videoAbsPath, screen);
        this.sController.setEnabled(true);
        this.vController.setEnabled(false);
    }


    public void Update()
    {
        if(this.aController == ActiveController.slide)
        {
            this.sController.Update();
        }
        else
        {

        }
    }

    public void toggleActiveController()
    {
        this.aController = this.aController == ActiveController.slide ? ActiveController.video : ActiveController.slide;
        this.sController.toggleEnabled();
        this.vController.toggleEnabled();
    }

/// <summary>
/// Slide interface: this just calls the SlideController Methods from a centralized location
/// </summary>
    public void incrementSlide()
    {
        this.sController.incrementSlide();
    }

    public void decrementSlide()
    {
        this.sController.decrementSlide();
    }

    public void resetSlide()
    {
        this.sController.resetSlide();
    }

    public bool slidesCompleted()
    {
        return  this.sController.slidesCompleted();
    }


    /// <summary>
    /// Video interface: this just calls the VideoController Methods from a centralized location
    /// </summary>
    public void play()
    {
        this.vController.play();
    }

    public void pause()
    {
        this.vController.pause();
    }

    public void restart()
    {
        this.vController.restart();
    }
}
