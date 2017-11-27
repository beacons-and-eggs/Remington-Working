using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

using ImageAndVideoPicker;

/***
 * 
 * 
 * 
 * 
 * 
 * 
 ***/

public class SlideViewerStateMachine : MonoBehaviour
{

    public string slideDirectory;//the folder where the slides live
    public string videoPath;//the path, including file name, abs or relative
    private SlideViewerStates currentState;

    public bool useFileChooser = true;
    
    public VideoPlayer video;
    public GameObject screen;
    public List<Sprite> slideSet;

    private ViewerController viewerController;

    private bool isBuilt = false;


    //            |--------------
    //            v             |
    // Intro -> Idle -> Slide --| 
    //               -> Video --|

    // Use this for initialization
    void Start()
    { 
        this.currentState = SlideViewerStates.Intro;
        if (useFileChooser)
        {
            Debug.Log("starting filechooser");
            viewerController = new ViewerController(screen, video);
            Debug.Log("Finished file chooser");

        }
        else
            viewerController = new ViewerController(slideSet, video, screen);
    }

    

    // Update is called once per frame
    // this state machine will eventually need to change 
    // to host changing between videoplayer and slideshow
    void Update()
    {

        if (isBuilt)
        {
            isBuilt = viewerController.buildUpdate();
        }
        else return;

        Debug.Log("Running");
        switch (currentState)
        {
            case SlideViewerStates.Intro:
                currentState = SlideViewerStates.Idle;
                break;
            case SlideViewerStates.Idle:
                currentState = SlideViewerStates.Slides;
                
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
                    Debug.Log("Moving to video");
                }
                break;
            case SlideViewerStates.Video:

                viewerController.play();

                break;
        }

    }


  

}
