using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Croutons_Dialogue_Simulator
{
    internal class DialogueBox
    {
        //The DialogueBox is basically just a box that takes up a big part of the screen, but it will need
        //a visual sprite to be passed in, a font to use, a sound effect to play when text is drawn, and a
        //system for scrolling text in the first place. My vision for it is to appear somewhat rapidly, one
        //letter at a time. I need a way to combine somewhat rigid text formatting with code to avoid having to
        //do 1000 individual draw calls for each letter (maybe it's as simple as a loop! Don't get discouraged)

        //Fields
        private Texture2D box;
        private Texture2D boxPortrait;
        private Rectangle positionRect;
        private SpriteFont font;
        private SoundEffect textSound;
        private DialogueChoice choice;
        private Color currentColor;
        bool effectPlayed = false;

        /// <summary>
        /// Constructor for the dialogue box. Complicated initially, but simplifies things later
        /// </summary>
        /// <param name="box">The sprite of the dialogue box itself</param>
        /// <param name="boxPortrait">The portrait to be displayed inside the dialogue box</param>
        /// <param name="positionRect">The positional rectangle of the dialogue box</param>
        /// <param name="font">The font used for this dialogue box instance</param>
        /// <param name="textSound">The text sound effect used for this dialogue box instance</param>
        public DialogueBox(Texture2D box, Texture2D boxPortrait, Rectangle positionRect, SpriteFont font, SoundEffect textSound)
        {
            this.box = box;
            this.boxPortrait = boxPortrait;
            this.positionRect = positionRect;
            this.font = font;
            this.textSound = textSound;
        }

        /// <summary>
        /// Get-set accessor for the sound effect. Will allow for reset of use (while sound effects are single use.
        /// </summary>
        public bool EffectPlayed
        {
            get { return effectPlayed; }
            set { effectPlayed = value; }
        }

        /// <summary>
        /// The method to display dialogue inside of the dialogue box
        /// </summary>
        /// <param name="sb">The spritebatcher to use</param>
        /// <param name="dialogue">The dialogue string to display inside of the box.</param>
        public void DisplayDialogue(SpriteBatch sb, string dialogue)
        {
            sb.Draw(box, positionRect, Color.White);
            sb.Draw(boxPortrait, new Rectangle(positionRect.X + 5, positionRect.Y + 10, boxPortrait.Width, boxPortrait.Height), Color.White);
            dialogue = DialogueFormatter(dialogue);
            sb.DrawString(font, dialogue, new Vector2(positionRect.X + 144, positionRect.Y + 30), Color.White);

            if (effectPlayed == false)
            {
                textSound.Play();
                effectPlayed = true;
            }
        }

        public void DisplayChoice(SpriteBatch sb, string[] options)
        {
            if (options.Length == 1)
            {
                sb.DrawString(font, options[0], new Vector2(positionRect.X + 144, positionRect.Y + 60), Color.White);
            }
            if (options.Length == 2)
            {
                sb.DrawString(font, options[0], new Vector2(positionRect.X + 144, positionRect.Y + 60), Color.White);
                sb.DrawString(font, options[1], new Vector2(positionRect.X + 244, positionRect.Y + 60), Color.White);
            }
            if (options.Length == 3)
            {
                sb.DrawString(font, options[0], new Vector2(positionRect.X + 144, positionRect.Y + 60), Color.White);
                sb.DrawString(font, options[1], new Vector2(positionRect.X + 244, positionRect.Y + 60), Color.White);
                sb.DrawString(font, options[2], new Vector2(positionRect.X + 144, positionRect.Y + 90), Color.White);
            }
            if (options.Length == 4)
            {
                sb.DrawString(font, options[0], new Vector2(positionRect.X + 144, positionRect.Y + 60), Color.White);
                sb.DrawString(font, options[1], new Vector2(positionRect.X + 244, positionRect.Y + 60), Color.White);
                sb.DrawString(font, options[2], new Vector2(positionRect.X + 144, positionRect.Y + 90), Color.White);
                sb.DrawString(font, options[3], new Vector2(positionRect.X + 244, positionRect.Y + 90), Color.White);
            }
        }


        /// <summary>
        /// A simple dialogue formatter that ensures dialogue cannot be outside of the dialogue box
        /// </summary>
        /// <param name="dialogue">The dialogue to be formatted</param>
        /// <returns>A formatted string of dialogue, adherant to the X-dimension bounds of the dialogue box</returns>
        public string DialogueFormatter(string dialogue)
        {
            //The line width starts at zero. It will increase for every word
            float lineWidth = 0;

            //The maximum width of any one line of dialogue is as long as the dialogue box (minus portrait space)
            int maxWidth = box.Width - boxPortrait.Width - 80;

            //The formatted string is the dialogue assuming that the if statement never catches
            StringBuilder formatter = new StringBuilder();

            //Sets up the character width dependent on font, and splits the list into an array of words
            float spaceWidth = font.MeasureString(" ").X;
            string[] words = dialogue.Split(" ");

            
            foreach (string word in words)
            {
                //Adds the length of the word (and space if applicable) to the linewidth
                float wordWidth = font.MeasureString(word).X + spaceWidth;
                lineWidth += wordWidth;
                
                //If the line is longer than the dialogue box, it will add a newline character BEFORE the offending word
                if (lineWidth > maxWidth)
                {
                    //makes sure the newline is added BEFORE the word that escapes the bounds.
                    formatter.Append("\n");
                    formatter.Append(word + " ");

                    //Linewidth reset
                    lineWidth = 0;
                }
                //Simply append the word if the line is within bounds
                else
                {
                    formatter.Append(word + " ");
                }
            }
            return formatter.ToString();
        }

        //Add a method for the formatting of questions and player-input scenarios
    }
    
}

