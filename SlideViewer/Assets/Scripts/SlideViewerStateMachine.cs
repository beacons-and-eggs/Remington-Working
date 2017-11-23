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


    private List<string> slidePathSet;

    private List<Sprite> slideSet;//
    public VideoPlayer video;
    public GameObject screen;
        

    private ViewerController viewerController;
    private bool isBuilt;

    //            |--------------
    //            v             |
    // Intro -> Idle -> Slide --| 
    //               -> Video --|

    // Use this for initialization
    void Start()
    {
        isBuilt = false;
        this.currentState = SlideViewerStates.Intro;

        //this can be skipped by providing you own
        Debug.Log("video path: " + videoPath);
        if(this.videoPath == null || videoPath == "")
        {
    
            #if UNITY_ANDROID
            
            AndroidPicker.BrowseVideo();
            #elif UNITY_IPHONE
			            IOSPicker.BrowseVideo(); // true for pick and crop
            #endif
            
            
        }

        Debug.Log("slide directory: " + slideDirectory);
        if (slideDirectory == null || slideDirectory == "")
        {
            #if UNITY_ANDROID

                    AndroidPicker.BrowseForMultipleImage();
            #elif UNITY_IPHONE
                    IOSPicker.BrowseImage(false); // true for pick and crop
#endif

        }
        
            
    }

    

    // Update is called once per frame
    // this state machine will eventually need to change 
    // to host changing between videoplayer and slideshow
    void Update()
    {
        //don't try and populate until the user has chosen files
        if (this.slidePathSet == null || this.videoPath == null)
            return;

        //the first time populate
        if (!isBuilt)
        {
            populateVideo();
            populateSlides();
            viewerController = new ViewerController(slideSet, video, screen);
            isBuilt = true;
        }

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


    /**
     * 
     * Grabs the video at the path and loads into video player
     * 
     *  @return true if succesful
     * 
     **/
    private bool populateVideo()
    {
        if(this.videoPath == null)
        {
            return false;
        }
        
        this.video.url = "file://" +  this.videoPath;

        return true;
    }



    /**
     * 
     * Grabs all png or jpg files in this slides path
     * 
     * @return true if successful
     * 
     **/
    private bool populateSlides()
    {
        List<string> slides = this.slidePathSet;
        
        //sort them in ascending order
        slides.Sort();

        //generate sprites from the slides
        List<Sprite> sprites = new List<Sprite>();
        for (int i = 0; i < slides.Count; i++)
        {
            Debug.Log("Generating Sprite " + (i + 1) + "/" + slides.Count);
            Texture2D texture = generateTexture(slides[i]);
            if (texture != null)
                sprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100f));
            else
                Debug.Log("bad texture");
        }

        this.slideSet = sprites;

        return slideSet.Count > 0;
    }





    /**
     * 
     * finds and retrieves the filepath/name of all files in a given directory
     * 
     * @param path string "the path to grab all files at"
     * 
     * @return List<string> this will be null if there was no imagePath specified
     *   
     */
    private List<string> findFiles(string path)
    {
        List<string> slides = null;
        if (this.slideDirectory == null)
        {
            Debug.Log("No slide path was provided.");

        }
        else
        {
            slides = new List<string>(Directory.GetFiles(path));

        }
        return slides;
    }



    /**
     * 
     * Generates a texture2d object given the filepath
     * Note: this will only work for .jpg or .png
     * 
     * @param filepath string "the location and name of the file to convert"
     * 
     * @return Texture2D object of the png/jpg
     * 
     **/
    private Texture2D generateTexture(string filePath)
    {
        Debug.Log("Generating textures for: " + filePath);
        Texture2D tex;
        byte[] fileData;
        //does the file exist
        if (File.Exists(filePath))
        {
            //get the raw data
            fileData = File.ReadAllBytes(filePath);
            //the initial size doesnt matter because it will get resized
            tex = new Texture2D(2, 2);
            //returns true if the file is valid
            if (tex.LoadImage(fileData))
            {
                return tex;
            }
        }
        Debug.Log("Texture Gen Returns Null");
        //return null if it doesnt exist
        return null;
    }




    void OnEnable()
    {
        PickerEventListener.onMultipleImageSelect += OnMultipleImageSelect;
        PickerEventListener.onVideoSelect += OnVideoSelect;

    }

    void OnDisable()
    {
        PickerEventListener.onMultipleImageSelect -= OnMultipleImageSelect;
        PickerEventListener.onVideoSelect -= OnVideoSelect;

    }


    void OnMultipleImageSelect(List<string> imgPath)
    {
        Debug.Log("Multiple images Selected");
        this.slidePathSet = imgPath;
        for (int i = 0; i < this.slidePathSet.Count; i++)
            Debug.Log("these are the results" + slidePathSet[i]);
    }

    void OnVideoSelect(string vidPath)
    {
        this.videoPath = vidPath;
        Debug.Log("Video Location : " + vidPath);
    }


}
