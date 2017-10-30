using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideViewerStateMachine : MonoBehaviour {

    public string slideLocation;//where do the slides live NOTE: USE ABSOLUTE PATH
    public string videoLocation;//where do the videos live NOTE: USE ABSOLUTE PATH
    private SlideViewerStates currentState;

    public GameObject screen;

    private SlideController sController;

//            |--------------
//            v             |
// Intro -> Idle -> Slide --| 
//               -> Video --|

	// Use this for initialization
	void Start () {
        this.currentState = SlideViewerStates.Intro;
        if (slideLocation != null)
            sController = new SlideController(this.slideLocation, screen);
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
                break;
            case SlideViewerStates.Slides:
                sController.Update();
                System.Threading.Thread.Sleep(100);
                sController.incrementSlide();
                break;
            case SlideViewerStates.Video:
                break;
        }
		
	}


}
