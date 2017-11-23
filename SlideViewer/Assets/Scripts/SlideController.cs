using System.Collections;
using System.Collections.Generic;
using System.IO;

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

    private int slideState = 0; //the current slide

    private List<Sprite> sprites; 


    private bool needToUpdateImage = true;
    private bool enabled = true;
    
    private Image canvasImage;
    
    public SlideController(List<Sprite> slides, GameObject screen)
    {
        this.sprites = slides;
        this.canvasImage = screen.GetComponentInChildren<Image>();

     
    }
	
	// Update is called once per frame
	public void Update () {
        //only update if the image has recently been changed
        if (enabled && needToUpdateImage)
        {
            Debug.Log("state: " + this.slideState);
            //create a new shell for Spirte
            Sprite s = this.sprites[this.slideState];
            Texture2D st = s.texture;
            //ensure that the file actually existed
            if (st == null)
                return;
            //create a basic sprite

            s = Sprite.Create(st, new Rect(0,0, st.width, st.height), new Vector2(0,0), 100f );

            //resize the image object to the picture size if necessary
            canvasImage.GetComponent<RectTransform>().sizeDelta = new Vector2(st.width, st.height);
            if (this.sprites == null)
                canvasImage.sprite = s;
            else
            {

                canvasImage.sprite = this.sprites[this.slideState];
                //this.sprites.RemoveAt(0);
            }

            needToUpdateImage = false;
        }
		
	}

    public bool slidesCompleted()
    {
        if (this.slideState == this.sprites.Count - 1)
            return true;
        return false;
    }

    public void incrementSlide()
    {
        if (this.enabled)
        {
            if (slideState + 1 < this.sprites.Count)
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





}
