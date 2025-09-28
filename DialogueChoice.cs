using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Croutons_Dialogue_Simulator
{
    /// <summary>
    /// Make this a class that uses an index based on an arrow inside a dialogue box.
    /// The arrow's index will be used to parse what option is being selected and how to respond.
    /// </summary>
    internal class DialogueChoice
    {
        private int choiceIndex;
        private DialogueBox dialogueBox;

        public DialogueChoice(DialogueBox displayBox)
        {
            dialogueBox = displayBox;
            choiceIndex = 0;
        }

        public void SingleChoice(SpriteBatch sb, string[] option, string[] response)
        {
            dialogueBox.DisplayChoice(sb, option);
            //Allow Choice (Program in player)
            //dialogueBox.DisplayDialogue(response based on choice)
        }
        public void MultiChoice(SpriteBatch sb, string[] options, string[] responses)
        {
            if (options.Length == 2 && responses.Length == 2)
            {
                dialogueBox.DisplayChoice(sb, options);
                //Allow Choice (Program in player)
                //dialogueBox.DisplayDialogue(response based on choice)
            }
            else if (options.Length == 3 && responses.Length == 3)
            {
                dialogueBox.DisplayChoice(sb, options);
                //Allow Choice (Program in player)
                //dialogueBox.DisplayDialogue(response based on choice)
            }
            else if (options.Length == 4 && responses.Length == 4)
            {
                dialogueBox.DisplayChoice(sb, options);
                //Allow Choice (Program in player)
                //dialogueBox.DisplayDialogue(response based on choice)
            }
        }
    }
}
