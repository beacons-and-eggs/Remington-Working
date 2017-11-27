using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

using ImageAndVideoPicker;

public class ViewerController : MonoBehaviour {

    private SlideController sController;
    private VideoController vController;
    private enum ActiveController { slide, video };
    private ActiveController aController;

    private string slideDirectory;//the folder where the slides live
    private string videoPath;//the path, including file name, abs or relative

    private List<string> slidePathSet;
    private List<Sprite> slideSet;
    public VideoPlayer video;
    public GameObject screen;

    private bool slidesBuilt = false;
    private bool videoBuilt = false;


    public ViewerController(GameObject screen, VideoPlayer video)
    {
        this.screen = screen;
        this.video = video;
        Debug.Log("about to start loading");
        loadSlides();
        Debug.Log("finished loading slides");
        loadVideo();
        this.aController = ActiveController.slide;
    }


 

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
        this.slidesBuilt = true;
        this.vController = new VideoController(video);
        this.videoBuilt = true;
        this.sController.setEnabled(true);
        this.vController.setEnabled(false);


    }


    public bool isBuilt()
    {
        return this.slidesBuilt && this.videoBuilt;
    }


    /**
     * 
     * 
     * 
     * 
     * 
     **/
    public void loadSlides()
    {
        Debug.Log("inside LoadSlides");
#if UNITY_ANDROID

        AndroidPicker.BrowseForMultipleImage();
        Debug.Log("after browse trigger");
#elif UNITY_IPHONE
                    IOSPicker.BrowseForMultipleImage(); // true for pick and crop
#endif

    }

    /**
     * 
     * 
     * 
     * 
     * 
     **/
    public void loadVideo()
    {
        if (this.videoPath == null || videoPath == "")
        {

#if UNITY_ANDROID

            AndroidPicker.BrowseVideo();
#elif UNITY_IPHONE
			            IOSPicker.BrowseVideo(); // true for pick and crop
#endif

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
        if (this.videoPath == null)
        {
            return false;
        }

        this.video.url = "file://" + this.videoPath;

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

    void Start()
    {

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
        Debug.Log("about to populate slides");
     
        Debug.Log("Finished populating slides");

        this.slidesBuilt = true;
    }

    void OnVideoSelect(string vidPath)
    {
        this.videoPath = vidPath;
        Debug.Log("OnVideoSelect Location : " + vidPath);
 
        
        this.videoBuilt = true;

    }

    public bool buildUpdate()
    {
        Debug.Log("viewer update");
        //don't try and populate until the user has chosen files
        if (this.slidePathSet == null || this.videoPath == null)
            return false;

        //the first time populate
        if (!isBuilt())
        {
            Debug.Log("building");
            populateSlides();
            this.sController = new SlideController(slideSet, screen);
            this.sController.setEnabled(true);
            populateVideo();
            this.vController = new VideoController(video);
            this.vController.setEnabled(false);
            return true;
        }
        return false;
    }

    public void Update()
    {
 
        //video player doesn't require a manual update
        if (this.aController == ActiveController.slide)
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
