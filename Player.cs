using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Enumeration for the player's state. The player cannot interact while moving or move while interacting, so they are mutually exclusive states
/// </summary>
public enum PlayerState
{
    Movement,
    Interact,
    Choice
}

public enum PlayerMoveState
{
    Still,
    Moving
}
public enum PlayerAnimState
{
    Front,
    Left,
    Right,
    Back
}

namespace Croutons_Dialogue_Simulator
{
    internal class Player
    {
        //Player fields (general)
        private Texture2D[] sprites;
        private Texture2D arrowIndicator;
        private PlayerState currentState;
        private Vector2 position;
        private Rectangle hitbox;
        private KeyboardState prevKeyState;
        private KeyboardState currentKeyState;

        //Player fields (animation)
        private Animation[] walkCycles;
        private PlayerMoveState moveState;
        private PlayerAnimState animationState;


        /// <summary>
        /// Accessor for this fucking arrow, which somehow is the most vital part of all of this
        /// </summary>
        public Texture2D ArrowIndicator
        {
            get { return arrowIndicator; }
        }

        /// <summary>
        /// Accessor for the position of the player
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Accessor for the player's hitbox
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        /// <summary>
        /// Accessor for the player's current action state
        /// </summary>
        public PlayerState CurrentState
        {
            get { return currentState; }
        }


        /// <summary>
        /// This will create a player at a preset position in the movement state
        /// </summary>
        /// <param name="sprite"></param>
        public Player(Texture2D[] sprites, Animation[] walkCycles, Texture2D arrowIndicator)
        {
            this.sprites = sprites;
            this.walkCycles = walkCycles;
            this.arrowIndicator = arrowIndicator;
            currentState = PlayerState.Movement;
            position = new Vector2(30, 30); //Arbitrary right now
            hitbox = new Rectangle((int)position.X + 12, (int)position.Y, sprites[0].Width - 24, sprites[0].Height);
            prevKeyState = Keyboard.GetState();
        }


        /// <summary>
        /// Allows the player to move, adjusting it's position and hitbox accordingly
        /// </summary>
        public void Move(Zone currentZone, int screenWidth, int screenHeight)
        {
            if(currentState == PlayerState.Movement)
            {
                currentKeyState = Keyboard.GetState();
                Vector2 playerMovement = Vector2.Zero;

                //Core movement details and state changes
                if (currentKeyState.IsKeyDown(Keys.W) && position.Y > 0)
                {
                    animationState = PlayerAnimState.Back;
                    playerMovement.Y -= 1;
                }
                if (currentKeyState.IsKeyDown(Keys.S) && position.Y < currentZone.PlayspaceY - sprites[0].Height)
                {
                    animationState = PlayerAnimState.Front;
                    playerMovement.Y += 1;
                }
                if (currentKeyState.IsKeyDown(Keys.A) && position.X > 0)
                {
                    animationState = PlayerAnimState.Left;
                    playerMovement.X -= 1;
                }
                if (currentKeyState.IsKeyDown(Keys.D) && position.X < currentZone.PlayspaceX - sprites[0].Width)
                {
                    animationState = PlayerAnimState.Right;
                    playerMovement.X += 1;
                }

                //Normalization and application of movement to hitboxes
                if (playerMovement != Vector2.Zero)
                {
                    playerMovement.Normalize();
                    position += playerMovement * 3;
                    position.X = (int)position.X;
                    position.Y = (int)position.Y;
                    hitbox.X = (int)position.X + 12;
                    hitbox.Y = (int)position.Y;
                    moveState = PlayerMoveState.Moving;
                }
                else
                {
                    moveState = PlayerMoveState.Still;
                }
            }
        }

        /// <summary>
        /// Updates the appropriate animation to the active animation state
        /// </summary>
        /// <param name="gametime"></param>
        public void AnimationSwitchyard(GameTime gametime)
        {
            if (animationState == PlayerAnimState.Front)
            {
                walkCycles[0].AnimateLoop(gametime);
                walkCycles[1].ResetAnimation();
                walkCycles[2].ResetAnimation();
                walkCycles[3].ResetAnimation();
            }

            if (animationState == PlayerAnimState.Right)
            {
                walkCycles[1].AnimateLoop(gametime);
                walkCycles[0].ResetAnimation();
                walkCycles[2].ResetAnimation();
                walkCycles[3].ResetAnimation();
            }

            if (animationState == PlayerAnimState.Back)
            {
                walkCycles[2].AnimateLoop(gametime);
                walkCycles[0].ResetAnimation();
                walkCycles[1].ResetAnimation();
                walkCycles[3].ResetAnimation();
            }

            if (animationState == PlayerAnimState.Left)
            {
                walkCycles[3].AnimateLoop(gametime);
                walkCycles[0].ResetAnimation();
                walkCycles[1].ResetAnimation();
                walkCycles[2].ResetAnimation();
            }
        }

        /// <summary>
        /// A method that handles collisions with the environment (or any static rectangle)
        /// </summary>
        /// <param name="intersect">The intersection rectangle of the player and an environment tile</param>
        public void EnvironmentCollisions(Rectangle intersect)
        {
            //X-axis collisions
            if (intersect.Width < intersect.Height)
            {
                //Right side collisions
                if (hitbox.X < intersect.X)
                {
                    hitbox.X -= intersect.Width;
                    
                }
                //Left side collisions
                if (hitbox.X - 24 > intersect.X - sprites[0].Width)
                {
                    hitbox.X += intersect.Width;
                }
            }

            //Y-axis collisions
            if (intersect.Width > intersect.Height)
            {

                //Top side collisions
                if (hitbox.Y < intersect.Y)
                {
                    hitbox.Y -= intersect.Height;
                }
                //Bottom side collisions
                if (hitbox.Y > intersect.Y - sprites[0].Height)
                {
                    hitbox.Y += intersect.Height;
                }
            }

            //Adjustment of position based on hitbox details
            position.X = hitbox.X - 12;
            position.Y = hitbox.Y;
        }

        /// <summary>
        /// A method that handles collisions between the player hitbox and an interaction circle
        /// </summary>
        /// <param name="interactCircle">The circle used in the collision logic</param>
        /// <returns>A boolean for the collision status</returns>
        public bool NPCCollisions(Circle interactCircle)
        {
            //Initializes test variables
            float testX = 0;
            float testY = 0;
            
            //Looks for the closest edge to the circle (top/bottom, left/right) and uses it as the testing variables

            //Left edge (rect)
            if(hitbox.X >= interactCircle.Center.X)
            {
                testX = hitbox.X;
            }
            //Right edge (rect)
            else if (hitbox.X < interactCircle.Center.X)
            {
                testX = hitbox.X + hitbox.Width;
            }
            //Top edge (rect)
            if(hitbox.Y >= interactCircle.Center.Y) 
            {
                testY = hitbox.Y;
            }
            //Bottom edge (rect)
            else if (hitbox.Y < interactCircle.Center.Y)
            {
                testY = hitbox.Y + hitbox.Height;
            }

            //Collisions are checked with the radius of the circle against the normalized distance between the center of the circle and the edge of the rect
            float distX = interactCircle.Center.X - testX;
            float distY = interactCircle.Center.Y - testY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));
            if (distance <= interactCircle.Radius)
            {
                return true; //Returns true for a collision
            }
            else
            {
                return false; //Returns false for no collision
            }
        }
        public int MakeYourSingleChoice(SpriteBatch sb, DialogueChoice theChoice)
        {
            if (currentState == PlayerState.Choice)
            {
                currentKeyState = Keyboard.GetState();
                DrawArrow(sb, new Vector2(theChoice.DialogueBox.PositionRect.X + 124, theChoice.DialogueBox.PositionRect.Y + 60));
                if (SingleKeyPress(Keys.Z, currentKeyState, prevKeyState))
                {
                    return 0;
                }
                prevKeyState = currentKeyState;
            }
            return 0;
        }

        public int MakeYourMultiChoice(SpriteBatch sb, DialogueChoice theChoice)
        {
            if (currentState == PlayerState.Choice)
            {
                currentKeyState = Keyboard.GetState();
                if (SingleKeyPress(Keys.W, currentKeyState, prevKeyState))
                {
                    if (theChoice.ChoiceIndex == 1)
                    {
                        theChoice.ChoiceIndex = 0;
                    }
                    if (theChoice.ChoiceIndex == 3)
                    {
                        theChoice.ChoiceIndex = 2;
                    }
                }
                if (SingleKeyPress(Keys.S, currentKeyState, prevKeyState))
                {
                    if (theChoice.ChoiceIndex == 0)
                    {
                        theChoice.ChoiceIndex = 1;
                    }
                    if (theChoice.ChoiceIndex == 2)
                    {
                        theChoice.ChoiceIndex = 3;
                    }
                }
                if (SingleKeyPress(Keys.D, currentKeyState, prevKeyState))
                {
                    if (theChoice.ChoiceIndex == 0)
                    {
                        theChoice.ChoiceIndex = 2;
                    }
                    if (theChoice.ChoiceIndex == 1)
                    {
                        theChoice.ChoiceIndex = 3;
                    }
                }
                if (SingleKeyPress(Keys.A, currentKeyState, prevKeyState))
                {
                    if (theChoice.ChoiceIndex == 2)
                    {
                        theChoice.ChoiceIndex = 0;
                    }
                    if (theChoice.ChoiceIndex == 3)
                    {
                        theChoice.ChoiceIndex = 1;
                    }
                }
                Vector2 pos = new Vector2(0, 0);
                if (theChoice.ChoiceIndex == 0)
                {
                    pos = new Vector2(theChoice.DialogueBox.PositionRect.X + 124, theChoice.DialogueBox.PositionRect.Y + 60);
                }
                if (theChoice.ChoiceIndex == 1)
                {
                    pos = new Vector2(theChoice.DialogueBox.PositionRect.X + 224, theChoice.DialogueBox.PositionRect.Y + 60);
                }
                if (theChoice.ChoiceIndex == 2)
                {
                    pos = new Vector2(theChoice.DialogueBox.PositionRect.X + 124, theChoice.DialogueBox.PositionRect.Y + 90);
                }
                if (theChoice.ChoiceIndex == 3)
                {
                    pos = new Vector2(theChoice.DialogueBox.PositionRect.X + 224, theChoice.DialogueBox.PositionRect.Y + 90);
                }
                DrawArrow(sb, pos);

                if (SingleKeyPress(Keys.Z, currentKeyState, prevKeyState))
                {
                    return theChoice.ChoiceIndex;
                }
                prevKeyState = currentKeyState;
            }
            
            //This should NEVER happen
            return -1;
        }

        /// <summary>
        /// A method that activates interaction with NPCs while in the movement state
        /// </summary>
        /// <param name="anyNPC">the NPC who's interact field is used</param>
        public void Interactivate(NPC anyNPC)
        {
            bool withinField = NPCCollisions(anyNPC.InteractField);
            if (withinField == true && currentState == PlayerState.Movement)
            {
                currentKeyState = Keyboard.GetState();
                if(SingleKeyPress(Keys.Z, currentKeyState, prevKeyState) && anyNPC.HasQuestion == false)
                {
                    currentState = PlayerState.Interact;
                }
                else if(SingleKeyPress(Keys.Z, currentKeyState, prevKeyState) && anyNPC.HasQuestion == true)
                {
                    currentState = PlayerState.Choice;
                }
                
                prevKeyState = currentKeyState;
            }
        }

        /// <summary>
        /// A method that activates the movement state while in the interaction state
        /// </summary>
        /// <param name="anyNPC">The NPC who's interact field is usedd</param>
        public void Interdeactivate(NPC anyNPC)
        {
            bool withinField = NPCCollisions(anyNPC.InteractField);
            if (withinField == true && currentState == PlayerState.Interact || currentState == PlayerState.Choice)
            {
                currentKeyState = Keyboard.GetState();
                if (SingleKeyPress(Keys.Z, currentKeyState, prevKeyState))
                {
                    //Resets the sound effect
                    anyNPC.NPCDialogueBox.EffectPlayed = false;

                    //If you've interacted once, you've interacted once.
                    if (anyNPC.InteractedOnce == false && anyNPC.InteractedTwice == false)
                    {
                        anyNPC.InteractedOnce = true;
                    }

                    //If you've interacted twice, you've interacted twice.
                    else if (anyNPC.InteractedOnce == true && anyNPC.InteractedTwice == false)
                    {
                        anyNPC.InteractedTwice = true;
                    }

                    //Changes the state back to movement
                    currentState = PlayerState.Movement;
                }
                prevKeyState = currentKeyState;
            }
        }

        /// <summary>
        /// A method that evaluates the validity of single key presses. This method helps ensure an action only happens once when a button is pressed.
        /// </summary>
        /// <param name="key">The key that is in question of being pressed</param>
        /// <param name="currentState">The state of the keyboard at the current frame</param>
        /// <param name="previousState">The state of the keyboard at the previous frame</param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys key, KeyboardState currentState, KeyboardState previousState)
        {
            if (currentState.IsKeyDown(key) == true && previousState.IsKeyDown(key) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (moveState == PlayerMoveState.Still)
            {
                DrawStill(sb);
            }
            else if (moveState == PlayerMoveState.Moving)
            {
                DrawAnimated(sb);
            }
        }



        /// <summary>
        /// Draws the player into the scene
        /// </summary>
        /// <param name="sb">the spritebatcher used to draw</param>
        public void DrawStill(SpriteBatch sb)
        {
            if (animationState == PlayerAnimState.Front)
            {
                sb.Draw(sprites[0],
                    new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height),
                    Color.White);
            }
            if (animationState == PlayerAnimState.Right)
            {
                sb.Draw(sprites[1],
                    new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height),
                    Color.White);
            }
            if (animationState == PlayerAnimState.Back)
            {
                sb.Draw(sprites[2],
                    new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height),
                    Color.White);
            }
            if (animationState == PlayerAnimState.Left)
            {
                sb.Draw(sprites[3],
                    new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height),
                    Color.White);
            }
        }

        /// <summary>
        /// Draws the walk cycles of the player into the scene
        /// </summary>
        /// <param name="sb">The standard Game1 spritebatcher to be used for drawing</param>
        public void DrawAnimated(SpriteBatch sb)
        {
            if (animationState == PlayerAnimState.Front)
            {
                walkCycles[0].DisplayAnimation(sb, new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height));
            }
            if (animationState == PlayerAnimState.Right)
            {
                walkCycles[1].DisplayAnimation(sb, new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height));
            }
            if (animationState == PlayerAnimState.Back)
            {
                walkCycles[2].DisplayAnimation(sb, new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height));
            }
            if (animationState == PlayerAnimState.Left)
            {
                walkCycles[3].DisplayAnimation(sb, new Rectangle((int)position.X, (int)position.Y, sprites[0].Width, sprites[0].Height));
            }
        }

        public void DrawArrow(SpriteBatch sb, Vector2 pos)
        {
            sb.Draw(arrowIndicator, pos, Color.White);
        }
        
    }
}
