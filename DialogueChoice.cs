using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Croutons_Dialogue_Simulator
{
    /// <summary>
    /// Make this a class that uses an index based on an arrow inside a dialogue box.
    /// The arrow's index will be used to parse what option is being selected and how to respond.
    /// </summary>
    internal class DialogueChoice
    {
        private int choiceIndex;
        private Player choiceMaker;
        private DialogueBox dialogueBox;

        public int ChoiceIndex
        {
            get { return choiceIndex; }
            set { choiceIndex = value; }
        }
        public DialogueBox DialogueBox
        {
            get { return dialogueBox; }
        }
        public DialogueChoice(Player choiceMaker, DialogueBox displayBox)
        {
            dialogueBox = displayBox;
            this.choiceMaker = choiceMaker;
            choiceIndex = 0; //Choices are currently locked to whatever this is set as
        }

        public void DisplayChoice(SpriteBatch sb, string[] options)
        {
            if (choiceMaker.SelectionMade == false)
            {
                if (options.Length == 1)
                {
                    sb.DrawString(dialogueBox.Font, options[0], new Vector2(dialogueBox.PositionRect.X + 144, dialogueBox.PositionRect.Y + 60), Color.White);
                }
                if (options.Length == 2)
                {
                    sb.DrawString(dialogueBox.Font, options[0], new Vector2(dialogueBox.PositionRect.X + 144, dialogueBox.PositionRect.Y + 60), Color.White);
                    sb.DrawString(dialogueBox.Font, options[1], new Vector2(dialogueBox.PositionRect.X + 244, dialogueBox.PositionRect.Y + 60), Color.White);
                }
                if (options.Length == 3)
                {
                    sb.DrawString(dialogueBox.Font, options[0], new Vector2(dialogueBox.PositionRect.X + 144, dialogueBox.PositionRect.Y + 60), Color.White);
                    sb.DrawString(dialogueBox.Font, options[1], new Vector2(dialogueBox.PositionRect.X + 244, dialogueBox.PositionRect.Y + 60), Color.White);
                    sb.DrawString(dialogueBox.Font, options[2], new Vector2(dialogueBox.PositionRect.X + 144, dialogueBox.PositionRect.Y + 90), Color.White);
                }
                if (options.Length == 4)
                {
                    sb.DrawString(dialogueBox.Font, options[0], new Vector2(dialogueBox.PositionRect.X + 144, dialogueBox.PositionRect.Y + 60), Color.White);
                    sb.DrawString(dialogueBox.Font, options[1], new Vector2(dialogueBox.PositionRect.X + 244, dialogueBox.PositionRect.Y + 60), Color.White);
                    sb.DrawString(dialogueBox.Font, options[2], new Vector2(dialogueBox.PositionRect.X + 144, dialogueBox.PositionRect.Y + 90), Color.White);
                    sb.DrawString(dialogueBox.Font, options[3], new Vector2(dialogueBox.PositionRect.X + 244, dialogueBox.PositionRect.Y + 90), Color.White);
                }
            }
        }

        public void SingleChoice(SpriteBatch sb, string[] option, string[] response)
        {
            DisplayChoice(sb, option);
            int zero = choiceMaker.MakeYourSingleChoice(sb, this);
            if (choiceMaker.SelectionMade == true)
            {
                dialogueBox.DisplayDialogue(sb, response[zero]);
            }
        }
        public void MultiChoice(SpriteBatch sb, string[] options, string[] responses)
        {
            if (options.Length == 2 && responses.Length == 2)
            {
                DisplayChoice(sb, options);
                int response = choiceMaker.MakeYourMultiChoice(sb, this);
                if (choiceMaker.SelectionMade == true)
                {
                    dialogueBox.DisplayDialogue(sb, responses[response]);
                }
            }
            else if (options.Length == 3 && responses.Length == 3)
            {
                DisplayChoice(sb, options);
                int response = choiceMaker.MakeYourMultiChoice(sb, this);
                if (choiceMaker.SelectionMade == true)
                {
                    dialogueBox.DisplayDialogue(sb, responses[response]);
                }
            }
            else if (options.Length == 4 && responses.Length == 4)
            {
                DisplayChoice(sb, options);
                int response = choiceMaker.MakeYourMultiChoice(sb, this);
                if (choiceMaker.SelectionMade == true)
                {
                    dialogueBox.DisplayDialogue(sb, responses[response]);
                }
            }
        }
    }
}
