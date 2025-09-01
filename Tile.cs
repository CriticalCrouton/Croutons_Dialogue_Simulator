using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Croutons_Dialogue_Simulator
{
    internal class Tile
    {
        //This will be a very simple class, only needing a texture, position, whether or not it is collidable, things like that
        //Make the TILE simple, keep the complexities for the Zone Code.

            private Texture2D sprite;
            private Vector2 position;
            private Rectangle hitbox;
            private bool collidable;

            /// <summary>
            /// Accessor for the background tile's sprite (should be used for drawing)
            /// </summary>
            public Texture2D Sprite
            {
                get { return sprite; }
            }

            /// <summary>
            /// Accessor for the background tile's position (should be used for drawing)
            /// </summary>
            public Vector2 Position
            {
                get { return position; }
                set { position = value; }
            }

            /// <summary>
            /// Accessor for the tile's hitbox
            /// </summary>
            public Rectangle Hitbox
            {
                get { return hitbox; }
            }

            /// <summary>
            /// Accessor for the background tile's collidable status. (should be used for collision logic)
            /// </summary>
            public bool Collidable
            {
                get { return collidable; }
            }

            /// <summary>
            /// Constructor for the background tile that takes in a sprite and position
            /// </summary>
            /// <param name="sprite">The sprite of the background tile</param>
            /// <param name="position">The position of the background tile</param>
            public Tile(Texture2D sprite, Vector2 position, bool collision)
            {
                this.sprite = sprite;
                this.position = position;
                hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
                collidable = collision;
            }

            /// <summary>
            /// A method to update the tile's hitbox via it's new position
            /// </summary>
            /// <param name="positionX">The new position of the tile in the x axis</param>
            /// <param name="positionY">The new position of the tile in the y axis</param>
            public void UpdateHitbox(int positionX, int positionY)
            {
                hitbox.X = positionX;
                hitbox.Y = positionY;
            }

            /// <summary>
            /// A method that allows a tile to draw itself at it's position.
            /// </summary>
            /// <param name="sb">The game1 SpriteBatch used for drawing.</param>
            public void Draw(SpriteBatch sb)
            {
                sb.Draw(sprite, position, Color.White);
            }
    }
}

