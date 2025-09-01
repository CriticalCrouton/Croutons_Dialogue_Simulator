using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Croutons_Dialogue_Simulator
{
    /// <summary>
    /// A class which will control animation of sprites based off of a spritesheet and elementary animation data. Can be looping or non-looping.
    /// </summary>
    internal class Animation
    {
        //Animation fields
        private Texture2D spriteSheet;
        private int numberOfSprites;
        private int spriteWidth;
        private int spriteHeight;
        private int rows;
        private int columns;
        private float secondsPerFrame;
        private double timeCounter;
        private int currentFrame;

        //Field for animation reset limitation
        private bool reset;

        //Animation properties
        public int SpriteWidth { get { return spriteWidth; } }
        public int SpriteHeight { get { return spriteHeight; } }

        /// <summary>
        /// Creates an Animation that can update and display a sprite sheet animation put to work
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to be used</param>
        /// <param name="numberOfSprites">The total number of sprites in the sheet</param>
        /// <param name="rows">The number of horizontal (X) rows of sprites</param>
        /// <param name="columns">The number of vertical (Y) columns of sprites</param>
        /// <param name="fps">The animation frames per second</param>
        public Animation(Texture2D spriteSheet, int numberOfSprites, int rows, int columns, int fps)
        {
            this.spriteSheet = spriteSheet;
            this.numberOfSprites = numberOfSprites;
            this.rows = rows;
            this.columns = columns;
            spriteWidth = spriteSheet.Width / (numberOfSprites / rows);
            spriteHeight = spriteSheet.Height / (numberOfSprites / columns);
            timeCounter = 0;
            secondsPerFrame = 1.0f / fps;
            currentFrame = 1;
            reset = true;
        }

        /// <summary>
        /// Animates a cycle in a loop. If done right, it should be seamless, but it depends on the sprite sheet loaded in
        /// </summary>
        /// <param name="gameTime"></param>
        public void AnimateLoop(GameTime gameTime)
        {
            if (reset == true)
            {
                reset = false;
            }
            //Elapsed time of the last active frame IN GAME
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                currentFrame++;
                if (currentFrame > numberOfSprites)
                {
                    currentFrame = 1;
                }

                // Reset the time counter, keeping remaining elapsed time
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Animates a cycle one time (without looping).
        /// Once the cycle is complete, it must be reset to ever happen again.
        /// </summary>
        /// <param name="gameTime"></param>
        public void AnimateOnce(GameTime gameTime)
        {
            //Elapsed time of the last active frame IN GAME
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                if (currentFrame >= numberOfSprites)
                {
                    currentFrame = numberOfSprites; //Does nothing
                }
                else
                {
                    currentFrame++;
                }
                // Reset the time counter, keeping remaining elapsed time
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Resets this animation to before it has run. (For single-cycle animations)
        /// </summary>
        public void ResetAnimation()
        {
            if (reset == false)
            {
                currentFrame = 1;
                timeCounter = 0;
            }
        }


        /// <summary>
        /// Draws the animation to the screen dependent on the position rectangle of the relevant subject
        /// </summary>
        /// <param name="sb">the spritebatcher used for drawing</param>
        /// <param name="positionRect">The position rectangle of the relevant entity</param>
        public void DisplayAnimation(SpriteBatch sb, Rectangle positionRect)
        {
            sb.Draw(spriteSheet, positionRect, new Rectangle(spriteWidth * (currentFrame - 1), 0, spriteWidth, spriteHeight), Color.White);
        }

    }
}
