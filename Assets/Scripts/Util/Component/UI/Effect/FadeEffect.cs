﻿using UnityEngine;

namespace Util.Component.UI.Fade
{
    public class FadeEffect
    {
        public void NextStep()
        {
            // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
            alpha += direction * speed * Time.deltaTime;

            // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
            alpha = Mathf.Clamp01(alpha);

            // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
            GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);

            // make the black texture render on top (drawn last)
            GUI.depth = drawDepth;

            // draw the texture to fit the entire screen area
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }

        #region Properties

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        #endregion

        #region Attributes
        
        readonly Texture2D texture;	// the texture that will overlay the screen. This can be a black image or a loading graphic

        float speed;		// the fading speed

        readonly int drawDepth;		// the texture's order in the draw hierarchy: a low number means it renders on top
	
        float alpha;	// the texture's alpha value between 0 and 1	

        readonly int direction;

        #endregion

        #region Constructors

        FadeEffect(Texture2D texture, float speed, int drawDepth, float alpha, int direction)
        {
            this.texture = texture;
            this.speed = speed;
            this.drawDepth = drawDepth;
            this.alpha = alpha;
            this.direction = direction;
        }

        public static FadeEffect In(Texture2D texture, float speed, int drawDepth)
        {
            return new FadeEffect(texture, speed, drawDepth, 1, -1);
        }
        
        public static FadeEffect Out(Texture2D texture, float speed, int drawDepth)
        {
            return new FadeEffect(texture, speed, drawDepth, 0, 1);
        }
        
        #endregion
    }
}