using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Croutons_Dialogue_Simulator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Test Zone Fields
        private Texture2D walkableTile;
        private Texture2D collidableTile;
        private Dictionary<string, Texture2D> testZoneSprites;
        private Zone testZone;
        private SystemCamera testCamera;
        private bool cameraFollowX;
        private bool cameraFollowY;

        //Test Player Fields
        private Texture2D[] playerSprites;
        private Animation[] playerWalkCycles;
        private Player player;

        //Test NPCs (and test dialoguebox)
        private NPC fruitcake;
        private Texture2D dialogueBoxTexture;
        private DialogueBox fruitcakeBox;
        private NPC probetheus;
        private DialogueBox probetheusBox;


        //Sound effects and spriteFonts
        private SoundEffect fruitcakeTextSound;
        private SpriteFont fruitcakeText;
        private SoundEffect probetheusTextSound;
        private SpriteFont probetheusText;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            cameraFollowX = true;
            cameraFollowY = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loading the Player
            playerSprites = new Texture2D[4];
            playerSprites[0] = Content.Load<Texture2D>("PlayerFront");      //Front - 0
            playerSprites[1] = Content.Load<Texture2D>("PlayerRight");      //Right - 1
            playerSprites[2] = Content.Load<Texture2D>("PlayerBack");       //Back - 2
            playerSprites[3] = Content.Load<Texture2D>("PlayerLeft");       //Left - 3

            Texture2D front = Content.Load<Texture2D>("PlayerFrontWalk");
            Texture2D right = Content.Load<Texture2D>("PlayerRightWalk");
            Texture2D back = Content.Load<Texture2D>("PlayerBackWalk");
            Texture2D left = Content.Load<Texture2D>("PlayerLeftWalk");

            playerWalkCycles = new Animation[4];
            playerWalkCycles[0] = new Animation(front, 4, 1, 4, 6);
            playerWalkCycles[1] = new Animation(right, 4, 1, 4, 6);
            playerWalkCycles[2] = new Animation(back, 4, 1, 4, 6);
            playerWalkCycles[3] = new Animation(left, 4, 1, 4, 6);

            Texture2D fuckinArrow = Content.Load<Texture2D>("InteractionArrow");

            player = new Player(playerSprites, playerWalkCycles, fuckinArrow);

            //Loading NPCs
            Texture2D fruitcakeSprite = Content.Load<Texture2D>("Fruitcake");
            Texture2D fruitcakeIdle = Content.Load<Texture2D>("FruitcakeIdleAnimation");
            Texture2D fruitcakePortrait = Content.Load<Texture2D>("FruitcakePortrait");
            Dictionary<string, string> fruitcakeTextDictionary = new Dictionary<string, string>();
            fruitcakeTextDictionary.Add("FIRST", "Sup");
            fruitcakeTextDictionary.Add("SECOND", "You like my hat?");
            fruitcakeTextDictionary.Add("THIRD", "Yeah you do.");
            dialogueBoxTexture = Content.Load<Texture2D>("DialogueBox");
            fruitcakeText = Content.Load<SpriteFont>("FruitcakeText");
            fruitcakeTextSound = Content.Load<SoundEffect>("BLORCH");
            fruitcakeBox = new DialogueBox(dialogueBoxTexture,
                                           fruitcakePortrait,
                                           new Rectangle(35, 300, dialogueBoxTexture.Width, dialogueBoxTexture.Height),
                                           fruitcakeText,
                                           fruitcakeTextSound);
            Animation fruitcakeIdleAnimation = new Animation(fruitcakeIdle, 4, 1, 4, 6);
            fruitcake = new NPC(fruitcakeSprite, fruitcakeIdleAnimation, fruitcakeTextDictionary, new Vector2(320, 32), false, fruitcakeBox);
            List<NPC> testNPCList = new List<NPC>();
            testNPCList.Add(fruitcake);

            Texture2D probetheusSprite = Content.Load<Texture2D>("Jimmy Bakersfield");
            Texture2D probetheusIdle = Content.Load<Texture2D>("JimmyBakersfieldIdleAnimation");
            Texture2D probetheusPortrait = Content.Load<Texture2D>("Jimmy Bakersfield Portrait");
            Dictionary<string, string> probetheusTextDictionary = new Dictionary<string, string>();
            probetheusTextDictionary.Add("FIRST", "I am Probetheus");
            probetheusTextDictionary.Add("SECOND", "Yes, you read that correctly");
            probetheusTextDictionary.Add("THIRD", "My name is actually Jimmy Bakersfield, but Probetheus gets a bigger reaction.");
            probetheusText = Content.Load<SpriteFont>("JimmyBakersfield");
            probetheusTextSound = Content.Load<SoundEffect>("Explosion Sound Effect 4");
            probetheusBox = new DialogueBox(dialogueBoxTexture,
                                            probetheusPortrait,
                                            new Rectangle(35, 300, dialogueBoxTexture.Width, dialogueBoxTexture.Height),
                                            probetheusText,
                                            probetheusTextSound);
            Animation probetheusIdleAnimation = new Animation(probetheusIdle, 6, 1, 6, 6);
            probetheus = new NPC(probetheusSprite, probetheusIdleAnimation, probetheusTextDictionary, new Vector2(320, 228), false, probetheusBox);
            testNPCList.Add(probetheus);

            Texture2D mork1Sprite = Content.Load<Texture2D>("MORK");
            Texture2D mork1Idle = Content.Load<Texture2D>("MORKAnim");
            Texture2D mork1Portrait = Content.Load<Texture2D>("MORKportrait");
            Dictionary<string, string> mork1TextDictionary = new Dictionary<string, string>();
            mork1TextDictionary.Add("PRECHOICE", "What's my name?");
            mork1TextDictionary.Add("ChoiceA", "MORK!");
            mork1TextDictionary.Add("ResponseA", "YES!!!!");
            SoundEffect mork1TextSound = Content.Load<SoundEffect>("MorkSound");
            DialogueBox mork1Box = new DialogueBox(dialogueBoxTexture,
                                                   mork1Portrait,
                                                   new Rectangle(35, 300, dialogueBoxTexture.Width, dialogueBoxTexture.Height),
                                                   fruitcakeText,
                                                   mork1TextSound);
            Animation mork1IdleAnimation = new Animation(mork1Idle, 4, 1, 4, 6);
            NPC mork1 = new NPC(mork1Sprite, mork1IdleAnimation, mork1TextDictionary, new Vector2(320, 468), true, mork1Box);
            testNPCList.Add(mork1);

            

            //Loading the Test Zone
            walkableTile = Content.Load<Texture2D>("WalkableTile");
            collidableTile = Content.Load<Texture2D>("CollidableTile");
            testZoneSprites = new Dictionary<string, Texture2D>();
            testZoneSprites.Add("walkable", walkableTile);
            testZoneSprites.Add("collidable", collidableTile);
            testZone = new Zone(player, testZoneSprites, testNPCList, 896, 896, 128);
            testCamera = new SystemCamera(testZone, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Player's movement logic runs every frame
            if (player.CurrentState == PlayerState.Movement)
            {
                player.Move(testZone, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                player.AnimationSwitchyard(gameTime);
                testCamera.Track(player);

                testZone.CollisionCheck(player);
                testZone.NPCAnimations(gameTime);
                foreach(NPC npc in testZone.NPCs)
                {
                    player.Interactivate(npc);
                }
            }
            else if (player.CurrentState == PlayerState.Interact || player.CurrentState == PlayerState.Choice)
            {
                foreach (NPC npc in testZone.NPCs)
                {
                    player.Interdeactivate(npc);
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            if (cameraFollowX == true)
            {
                _spriteBatch.Begin(transformMatrix: testCamera.Transform);

                //TestZone is drawn in
                testZone.Draw(_spriteBatch);

                //Player is drawn in (on top of zone)
                player.Draw(_spriteBatch);
                DebugLibrary.DrawRectOutline(_spriteBatch, player.Hitbox, 3.0f, Color.Red);

                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.Begin();

                //TestZone is drawn in
                testZone.Draw(_spriteBatch);

                //Player is drawn in (on top of zone)
                player.Draw(_spriteBatch);
                DebugLibrary.DrawRectOutline(_spriteBatch, player.Hitbox, 3.0f, Color.Red);

                _spriteBatch.End();
            }

            _spriteBatch.Begin();
            //If an interaction is going on, it is drawn here.
            foreach (NPC npc in testZone.NPCs)
            {
                if (player.NPCCollisions(npc.InteractField) == true)
                {
                    npc.TalkToEm(_spriteBatch, player);
                }
            }

            _spriteBatch.DrawString(fruitcakeText, $"PlayerX: {player.Position.X}", new Vector2(10, 10), Color.Purple);
            _spriteBatch.DrawString(fruitcakeText, $"PlayerY: {player.Position.Y}", new Vector2(10, 25), Color.Purple);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
