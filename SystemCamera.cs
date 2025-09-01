using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Croutons_Dialogue_Simulator
{
    /// <summary>
    /// A class that uses the current zones boundaries to create a player-tracking camera system that
    /// seamlessly follows the player while disallowing viewership of unused space
    /// </summary>
    internal class SystemCamera
    {
        //Camera fields
        private Matrix transform;
        private int screenWidth;
        private int screenHeight;
        private Zone zoneOfCamera;

        //Camera properties
        public Matrix Transform
        {
            get { return transform; }
        }

        /// <summary>
        /// Standard constructor for the system camera
        /// </summary>
        /// <param name="zone">The zone that the camera is located in</param>
        /// <param name="screenwidth">The width of the game window</param>
        /// <param name="screenheight">The height of the game window</param>
        public SystemCamera(Zone zone, int screenwidth, int screenheight)
        {
            zoneOfCamera = zone;
            screenWidth = screenwidth;
            screenHeight = screenheight;
        }

        /// <summary>
        /// Keeps the player in the center of the screen, unless you are approaching the edges of the screen.
        /// Works in either direction independently
        /// </summary>
        /// <param name="player">The player object that the camera is meant to follow</param>
        public void Track(Player player)
        {
            float PosX = -player.Position.X + (screenWidth / 2) - (player.Hitbox.Width / 2);
            PosX = MathHelper.Clamp(PosX, -zoneOfCamera.PlayspaceX + (screenWidth), 0);
            float PosY = -player.Position.Y + (screenHeight / 2) - (player.Hitbox.Height / 2);
            PosY = MathHelper.Clamp(PosY, -zoneOfCamera.PlayspaceY + (screenHeight), 0);

            transform = Matrix.CreateTranslation(PosX,
                                                 PosY,
                                                 0);
        }
    }
}

