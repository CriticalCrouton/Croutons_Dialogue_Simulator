using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Croutons_Dialogue_Simulator
{
    internal class Zone
    {
        //This will be the class for the creation of zones. It will be constructor heavy, probably with few public methods
        //A lot of the code can probably be ripped and modded from my Environment class in the 106 game (Space Monkey Repo)
        //Zones should involve walkable space, but also collidable tiles (hitboxes for that should be very simple).
        //Zones will need to be made of tiles to use some of my more advanced level code (if so, there should be collidable tiles)
        //To simplify zones, file I/O can be used. That will probably be the part requiring the most modding from my original code.

        //fields
        private Tile[,] level;
        private List<NPC> NPCList;
        private int playspaceX;
        private int playspaceY;

        /// <summary>
        /// The accessor for the level array of background tiles
        /// </summary>
        public Tile[,] Level
        {
            get { return level; }
        }

        /// <summary>
        /// An accessor for the NPC list. This is used to help facilitate NPCs scrolling with the environment
        /// </summary>
        public List<NPC> NPCs
        {
            get { return NPCList; }
            set { NPCList = value; }
        }

        /// <summary>
        /// Get-only accessor for the width of the playable space
        /// </summary>
        public int PlayspaceX
        {
            get { return playspaceX; }
        }

        /// <summary>
        /// Get-only accessor for the height of the playable space
        /// </summary>
        public int PlayspaceY
        {
            get { return playspaceY; }
        }


        /// <summary>
        /// A constructor for the playspace background of the game
        /// </summary>
        /// <param name="sprites">A dictionary of sprites. The key for those sprite keys can be found in the level map </param>
        /// <param name="playspaceX">The width of the playspace</param>
        /// <param name="playspaceY">The height of the playspace</param>
        /// <param name="tilesize">The width/length in pixels of a SQUARE tile (width = height)</param>
        public Zone(Player player, Dictionary<string, Texture2D> sprites, List<NPC> NPCs, int playspaceX, int playspaceY, int tilesize)
        {
            this.playspaceX = playspaceX;
            this.playspaceY = playspaceY;

            int rowCount = playspaceX / tilesize; //It's important to use playspace dimensions that are divisible by the size of the tiles!
            int columnCount = playspaceY / tilesize; //Otherwise, things will not work correctly and errors could arise.

            //Opens the streamreader
            StreamReader levelMapReader = new StreamReader("../../../Content/LevelMap.txt");

            //Creates the level array
            level = new Tile[playspaceX / tilesize, playspaceY / tilesize];
            NPCList = NPCs;

            //Initializes a y-accessor.
            int y = 0;

            //Initial line read
            string readLine = levelMapReader.ReadLine();

            //Continuously reads lines until there is nothing left to read
            while (readLine != null)
            {
                //Passes over anything with a bypass character, or empty lines.
                if (readLine.StartsWith('/') || readLine == "")
                {
                    readLine = levelMapReader.ReadLine();
                }
                //Reads in by row and breaks tiles down into collidable or non-collidable based on their character representation.
                else
                {
                    for (int x = 0; x < rowCount; x++)
                    {
                        string currentChar = readLine.Substring(x, 1);
                        if (currentChar == "#")
                        {
                            level[x, y] = new Tile(sprites["walkable"], new Vector2(x * tilesize, y * tilesize), false);
                        }
                        else if (currentChar == "*")
                        {
                            level[x, y] = new Tile(sprites["collidable"], new Vector2(x * tilesize, y * tilesize), true);
                        }
                        //If there is a character not recognized by the system, an exception will be thrown.
                        else if (currentChar != "#" && currentChar != "*")
                        {
                            throw new Exception("The level editor has not been formatted correctly");
                        }
                    }
                    //Increments the y-indexer and moves on to the next line.
                    readLine = levelMapReader.ReadLine();
                    y++;
                }
            }
            //Closes the reader once it's job is done
            if (levelMapReader != null)
            {
                levelMapReader.Close();
            }
            //Consult with Josh about how the camera works.
        }

        /// <summary>
        /// Draws the background to the screen (should be called in Game1.Draw()
        /// </summary>
        /// <param name="sb">The Game1 SpriteBatch used to draw</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in level)
            {
                tile.Draw(sb);
            }
            foreach (NPC npc in NPCList)
            {
                if (npc.AnimationState == NPCstate.Idle)
                {
                    npc.Idle.DisplayAnimation(sb, npc.Hitbox);
                    DebugLibrary.DrawCircleOutline(sb, npc.InteractField.Center, npc.InteractField.Radius, 70, 3.0f, Color.Red);
                    DebugLibrary.DrawRectOutline(sb, npc.Hitbox, 3.0f, Color.Blue);
                }
                else
                {
                    npc.Draw(sb); //Temporary solution
                }
            }
        }

        /// <summary>
        /// Checks all tiles and NPCs in the zone for collisions with the player
        /// </summary>
        /// <param name="player">The player</param>
        public void CollisionCheck(Player player)
        {
            foreach(Tile tile in level)
            {
                //Only runs if the tile is collidable and the player is colliding with it
                if (tile.Collidable == true && player.Hitbox.Intersects(tile.Hitbox))
                {
                    Rectangle intersect = Rectangle.Intersect(tile.Hitbox, player.Hitbox);
                    player.EnvironmentCollisions(intersect);
                }
            }
            foreach(NPC npc in NPCList)
            {
                if(player.Hitbox.Intersects(npc.Hitbox))
                {
                    Rectangle intersect = Rectangle.Intersect(npc.Hitbox, player.Hitbox);
                    //The name is a tad bit confusing, but this is the best way for all NPC hitboxes to be considered at once
                    player.EnvironmentCollisions(intersect);
                }
            }
        }


        /// <summary>
        /// Animates all NPCs located inside of this zone
        /// </summary>
        /// <param name="gametime">the standard game1 gametime variable</param>
        public void NPCAnimations(GameTime gametime)
        {
            foreach (NPC npc in NPCList)
            {
                if (npc.AnimationState == NPCstate.Idle)
                {
                    npc.Idle.AnimateLoop(gametime);
                }
                else
                {
                    //This will not happen right now as I have no active animations
                }
            }
        }
    }
}

