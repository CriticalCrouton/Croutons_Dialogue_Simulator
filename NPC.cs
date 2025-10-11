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

    public enum NPCstate
    {
        Idle,
        Interacting
    }

    /// <summary>
    /// The class for interactable NPCs. Refer inside the class for the dictionary key-standardization
    /// </summary>
    internal class NPC
    {
        /// <KeyStandardizationGuide>
        /// KEY = FIRST: Introductory dialogue (said when interactedOnce and interactedTwice are BOTH false)
        /// KEY = SECOND: Secondary dialogue (said when interactedTwice is false BUT interactedOnce is true)
        /// KEY = THIRD: Finalized dialogue (said when interactedOnce and interactedTwice are BOTH true)
        /// KEY = PRECHOICE: If there is an interaction choice present, this will display before it.
        /// KEY = ChoiceA: If there is an interaction choice present in dialogue, this is the FIRST one
        /// KEY = ChoiceB: If there is an interaction choice present in dialogue, this is the SECOND one
        /// KEY = ChoiceC: If there is an interaction choice present in dialogue, this is the THIRD one
        /// KEY = ChoiceD: If there is an interaction choice present in dialogue, this is the FOURTH one
        /// KEY = ResponseA: If there is an interaction choice present in dialogue, this is the FIRST response
        /// KEY = ResponseB: If there is an interaction choice present in dialogue, this is the SECOND response
        /// KEY = ResponseC: If there is an interaction choice present in dialogue, this is the THIRD response
        /// KEY = ResponseD: If there is an interaction choice present in dialogue, this is the FOURTH response
        /// You do NOT HAVE to have all of these options in an NPC's dialogue dictionary
        /// You DO HAVE to stick to this formatting for these options
        /// </KeyStandardizationGuide>


        //NPC fields (Dialogue)
        private Dictionary<string, string> dialogue;
        private bool interactedOnce;
        private bool interactedTwice;

        //NPC fields (Interactable)
        private Texture2D sprite;
        private Animation idle;
        private Vector2 position;
        private Rectangle hitbox;
        private Circle interactField;
        private bool hasQuestion;
        private DialogueBox NPCbox;

        //NPC fields (Animation(s));
        private Animation idleAnimation;
        private NPCstate animationState;


        //Properties
        public Vector2 Position { get { return position; } set { position = value; } }
        public Rectangle Hitbox { get { return hitbox; } }
        public Circle InteractField { get { return interactField; } }
        public bool InteractedOnce { get { return interactedOnce; } set { interactedOnce = value; } }
        public bool InteractedTwice { get { return interactedTwice; } set { interactedTwice = value; } }
        public bool HasQuestion { get { return hasQuestion; } set { hasQuestion = value; } }
        public DialogueBox NPCDialogueBox { get { return NPCbox; } }
        public NPCstate AnimationState { get { return animationState; } set { animationState = value;} }
        public Animation Idle { get { return idleAnimation; } }


        /// <summary>
        /// The constructor for an NPC. The dialogue dictionary will have to be made PRIOR to the NPC
        /// </summary>
        /// <param name="sprite">The NPC's sprite</param>
        /// <param name="dialogue">The dialogue dictionary for the NPC</param>
        /// <param name="location">The location that the NPC starts at</param>
        public NPC(Texture2D sprite, Animation idle, Dictionary<string, string> dialogue, Vector2 location,bool hasQuestion, DialogueBox aDBox)
        {
            //Animation details
            this.sprite = sprite;
            idleAnimation = idle;
            animationState = NPCstate.Idle;

            //Dialogue details
            this.dialogue = dialogue;
            interactedOnce = false;
            interactedTwice = false;
            this.hasQuestion = hasQuestion;
            NPCbox = aDBox;

            //Functional information
            position = location;
            interactField = new Circle((sprite.Width), new Vector2(position.X + (sprite.Width / 2), position.Y + (sprite.Height / 2)));
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }


        /// <summary>
        /// The wildly incomplete method for the NPC interactions with the player
        /// </summary>
        /// <param name="player">The user-controlled player</param>
        public void TalkToEm(SpriteBatch sb, Player player)
        {
            //Basic "talking" NPCs.
            if (player.CurrentState == PlayerState.Interact)
            {
                if (interactedOnce == false && interactedTwice == false)
                {
                    NPCbox.DisplayDialogue(sb, dialogue["FIRST"]);
                }
                
                //Handles three-sentence NPCS
                if (dialogue.ContainsKey("SECOND") && dialogue.ContainsKey("THIRD"))
                {
                    if (interactedOnce == true && interactedTwice == false)
                    {
                        NPCbox.DisplayDialogue(sb, dialogue["SECOND"]);
                    }
                    if (interactedOnce == true && interactedTwice == true)
                    {
                        NPCbox.DisplayDialogue(sb, dialogue["THIRD"]);
                    }
                }
                //Handles two-sentence NPCs
                else if (dialogue.ContainsKey("SECOND") && dialogue.ContainsKey("THIRD") == false)
                {
                    if (interactedOnce == true)
                    {
                        NPCbox.DisplayDialogue(sb, dialogue["SECOND"]);
                    }
                }
            }

            //Active "Talking" NPCs.
            if (player.CurrentState == PlayerState.Choice)
            {
                if(dialogue.ContainsKey("PRECHOICE"))
                {
                    NPCbox.DisplayDialogue(sb, dialogue["PRECHOICE"]);
                }
                //One-option
                if (dialogue.ContainsKey("ChoiceA") && dialogue.ContainsKey("ChoiceB") == false && dialogue.ContainsKey("ChoiceC") == false && dialogue.ContainsKey("ChoiceD") == false)
                {
                    DialogueChoice oneChoice = new DialogueChoice(player, NPCbox);
                    string[] choices = { dialogue["ChoiceA"]};
                    string[] responses = { dialogue["ResponseA"]};
                    oneChoice.SingleChoice(sb, choices, responses);
                }
                //Two-option
                if (dialogue.ContainsKey("ChoiceA") && dialogue.ContainsKey("ChoiceB") && dialogue.ContainsKey("ChoiceC") == false && dialogue.ContainsKey("ChoiceD") == false)
                {
                    DialogueChoice twoChoice = new DialogueChoice(player, NPCbox);
                    string[] choices = { dialogue["ChoiceA"], dialogue["ChoiceB"] };
                    string[] responses = { dialogue["ResponseA"], dialogue["ResponseB"] };
                    twoChoice.MultiChoice(sb, choices, responses);
                }
                //Three-option
                if (dialogue.ContainsKey("ChoiceA") && dialogue.ContainsKey("ChoiceB") && dialogue.ContainsKey("ChoiceC") && dialogue.ContainsKey("ChoiceD") == false)
                {
                    DialogueChoice threeChoice = new DialogueChoice(player, NPCbox);
                    string[] choices = { dialogue["ChoiceA"], dialogue["ChoiceB"], dialogue["ChoiceC"] };
                    string[] responses = { dialogue["ResponseA"], dialogue["ResponseB"], dialogue["ResponseC"] };
                    threeChoice.MultiChoice(sb, choices, responses);
                }
                //Four-option
                if (dialogue.ContainsKey("ChoiceA") && dialogue.ContainsKey("ChoiceB") && dialogue.ContainsKey("ChoiceC") && dialogue.ContainsKey("ChoiceD"))
                {
                    DialogueChoice fourChoice = new DialogueChoice(player, NPCbox);
                    string[] choices = { dialogue["ChoiceA"], dialogue["ChoiceB"], dialogue["ChoiceC"], dialogue["ChoiceD"] };
                    string[] responses = { dialogue["ResponseA"], dialogue["ResponseB"], dialogue["ResponseC"], dialogue["ResponseD"] };
                    fourChoice.MultiChoice(sb, choices, responses);
                }
            }
        }


        /// <summary>
        /// A method that updates an NPCs hitbox and interaction field in accordance with the world movement
        /// </summary>
        /// <param name="positionX">The new position of an NPC in the x direction</param>
        /// <param name="positionY">The new position of an NPC in the y direction</param>
        public void UpdateShapes(int positionX, int positionY)
        {
            hitbox.X = positionX;
            hitbox.Y = positionY;
            interactField = new Circle(interactField.Radius, new Vector2(positionX + (hitbox.Width / 2), positionY + (hitbox.Height / 2)));
        }

        /// <summary>
        /// Draws the NPC (and their interaction field)
        /// </summary>
        /// <param name="sb">The spritebatcher used to draw</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
            DebugLibrary.DrawCircleOutline(sb, interactField.Center, interactField.Radius, 70, 3.0f, Color.Red);
            DebugLibrary.DrawRectOutline(sb, hitbox, 3.0f, Color.Blue);
        }
        //The NPC will need storage for their dialogue through a key-standardized dictionary, as well as an interacted-with bool
        //The NPC is important to the text box, but doesn't really do anything themselves. I will need to do most of my work on
        //animations for NPCs. Even if it's just a simple breath-bounce, they will look really odd if they are just standing still
        //The writing for these guys is the most important part.
    }
}
