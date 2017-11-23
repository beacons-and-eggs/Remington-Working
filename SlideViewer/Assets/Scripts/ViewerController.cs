using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class ViewerController {

    private SlideController sController;
    private VideoController vController;
    private enum ActiveController { slide, video };
    private ActiveController aController;

    /**
     * 
     * Viewer Controller constructor
     * 
     * @param slides List<Sprite> "the sprite objects for the slides"
     * 
     * @param video VideoPlayer "The video object preloaded with video" 
     * 
     * @param screen GameObject "the 2d plane that hosts the slides"
     * 
     **/
    public ViewerController(List<Sprite> slides, VideoPlayer video, GameObject screen)
    {
        this.aController = ActiveController.slide;

        this.sController = new SlideController(slides, screen);
        this.vController = new VideoController(video);
        this.sController.setEnabled(true);
        this.vController.setEnabled(false);
    }


    public void Update()
    {
       //video player doesn't require a manual update
        if(this.aController == ActiveController.slide)
        {
            this.sController.Update();
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
