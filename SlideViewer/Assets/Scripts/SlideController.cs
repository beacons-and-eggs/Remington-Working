using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// How To Use:
///     SlideController sController = new SlideController(this.slideLocation, screen);
///     loop
///         sController.Update(); 
///         s.incrementSlide();
///     end
/// </summary>



public class SlideController
{

    private string slideAbsPath; // the general location of the slides
    private int slideState = 0; //the current slide
    private List<string>  slides; //the location and names of the slides


    private bool needToUpdateImage = true;
    private bool enabled = true;
    
    private Image canvasImage;
    
    public SlideController(string slideAbsPath, GameObject screen)
    {
        this.slideAbsPath = slideAbsPath;
        this.canvasImage = screen.GetComponentInChildren<Image>();

        findSlides(); //get all files in the slideAbsPath directory
        scrubSlides(); //only hold on to png or jpg
     
    }
	
	// Update is called once per frame
	public void Update () {
        //only update if the image has recently been changed
        if (enabled && needToUpdateImage)
        {
            //find where this image is
            //Debug.Log(this.slideState + " " + (this.slides.Count-1)+ " " + this.slides[this.slideState]);
            string loc =   this.slides[this.slideState];

            //create a new shell for Spirte
            Sprite s = new Sprite();
            Texture2D st = LoadTexture(loc);
            //ensure that the file actually existed
            if (st == null)
                return;
            //create a basic sprite
            s = Sprite.Create(st, new Rect(0,0, st.width, st.height), new Vector2(0,0), 100f );

            //resize the image object to the picture size if necessary
            canvasImage.GetComponent<RectTransform>().sizeDelta = new Vector2(st.width, st.height);
            canvasImage.sprite = s;

            needToUpdateImage = false;
        }
		
	}

    public bool slidesCompleted()
    {
        if (slideState + 1 == this.slides.Count)
            return true;
        return false;
    }

    public void incrementSlide()
    {
        if (this.enabled)
        {
            if (slideState + 1 < this.slides.Count)
            {
                needToUpdateImage = true;
                slideState++;
            }
        }

    }

    public void decrementSlide()
    {
        if (this.enabled)
        {
            if (slideState - 1 >= 0)
            {
                needToUpdateImage = true;
                slideState--;
            }
        }
    }

    public void resetSlide()
    {
        if (this.enabled)
        {
            needToUpdateImage = true;
            this.slideState = 0;
        }
    }

    public void setEnabled(bool enabled)
    {
        this.enabled = enabled;
        this.canvasImage.enabled = this.enabled;
    }

    public void toggleEnabled()
    {
        this.enabled = !this.enabled;
        this.canvasImage.enabled = this.enabled;
        
    }



        //returns a Texture2D if the file exists, null if it doesn't
    private Texture2D LoadTexture(string filePath)
    {
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
        //return null if it doesnt exist
        return null;
    }


    private bool findSlides()
    {
        this.slides = new List<string>(Directory.GetFiles(this.slideAbsPath));
        return slides.Count > 0;
    }

    private bool scrubSlides()
    {
        for (int i = 0; i < slides.Count; i++)
        {
            if (!(Path.GetExtension(slides[i]).Equals(".png") || Path.GetExtension(slides[i]).Equals(".jpg") || Path.GetExtension(slides[i]).Equals(".PNG")))
            {
                slides.RemoveAt(i);
                continue;
            }
        }
        this.slides.Sort();

        return this.slides.Count > 0;
    }
}
