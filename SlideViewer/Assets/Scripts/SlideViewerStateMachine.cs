using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideViewerStateMachine : MonoBehaviour {

    public string slideAbsPath;//where do the slides live NOTE: USE ABSOLUTE PATH
    public string videoAbsPath;//where do the videos live NOTE: USE ABSOLUTE PATH
    private SlideViewerStates currentState;

    public GameObject screen;

    private ViewerController viewerController;

//            |--------------
//            v             |
// Intro -> Idle -> Slide --| 
//               -> Video --|

	// Use this for initialization
	void Start () {
        this.currentState = SlideViewerStates.Intro;
        if (slideAbsPath != null && videoAbsPath != null)
            viewerController = new ViewerController(this.slideAbsPath, this.videoAbsPath , screen);
	}
	
	// Update is called once per frame
    // this state machine will eventually need to change 
    // to host changing between videoplayer and slideshow
	void Update () {

        switch (currentState)
        {
            case SlideViewerStates.Intro:
                currentState = SlideViewerStates.Idle;
                break;
            case SlideViewerStates.Idle:
                currentState = SlideViewerStates.Slides;
                viewerController.incrementSlide();
                viewerController.Update();
                break;
            case SlideViewerStates.Slides:
                viewerController.incrementSlide();
                viewerController.Update();
                System.Threading.Thread.Sleep(100);
                ;
                if (viewerController.slidesCompleted())
                {
                    viewerController.toggleActiveController();
                    currentState = SlideViewerStates.Video;
                }
                break;
            case SlideViewerStates.Video:

                viewerController.play();

                break;
        }
		
	}


}
