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
    /// <summary>
    /// A struct for the usage of circles and circular "fields"
    /// </summary>
    internal struct Circle
    {
        //Fields
        private float radius;
        private Vector2 center;


        //Get-only properties
        public float Radius
        {
            get { return radius; }
        }
        public Vector2 Center
        {
            get { return center; }
        }

        /// <summary>
        /// Constructor for the circular object
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="center">The center of the circle</param>
        public Circle(float radius, Vector2 center)
        {
            this.radius = radius;
            this.center = center;
        }
    }
}
