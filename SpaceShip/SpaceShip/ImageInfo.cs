using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShip
{
    /// <summary>
    /// A class for an image info
    /// </summary>
    class ImageInfo
    {
        #region Fields

        Vector2 origin;
        Point size, dimensions;
        int radius;
        int lifeSpan;
        bool animated;

        #endregion

        #region Constructors

        /// <summary>
        ///  Contains an image info
        /// </summary>
        /// <param name="origin">the origin of the sprite</param>
        /// <param name="size">the size the sprite</param>
        /// <param name="dimensions">the dimensions of the tile</param>
        /// <param name="radius">the radius of the sprite</param>
        public ImageInfo(Vector2 origin, Point size, Point dimensions, int radius)
        {
            this.origin = origin;
            this.size = size;
            this.dimensions = dimensions;
            this.radius = radius;
            this.lifeSpan = int.MaxValue;
            this.animated = false;
        }

        /// <summary>
        ///  Contains an image info
        /// </summary>
        /// <param name="origin">the origin of the sprite</param>
        /// <param name="size">the size the sprite</param>
        /// <param name="dimensions">the dimensions of the tile</param>
        /// <param name="radius">the radius of the sprite</param>
        /// <param name="lifeSpan">the lifespan of the sprite</param>
        public ImageInfo(Vector2 origin, Point size, Point dimensions, int radius, int lifeSpan)
        {
            this.origin = origin;
            this.size = size;
            this.dimensions = dimensions;
            this.radius = radius;
            this.lifeSpan = lifeSpan;
            this.animated = false;
        }

        /// <summary>
        ///  Contains an image info
        /// </summary>
        /// <param name="origin">the origin of the sprite</param>
        /// <param name="size">the size the sprite</param>
        /// <param name="dimensions">the dimensions of the tile</param>
        /// <param name="radius">the radius of the sprite</param>
        /// <param name="lifeSpan">the lifespan of the sprite</param>
        /// <param name="animated">if the sprite is animated</param>
        public ImageInfo(Vector2 origin, Point size, Point dimensions, int radius, int lifeSpan, bool animated)
        {
            this.origin = origin;
            this.size = size;
            this.dimensions = dimensions;
            this.radius = radius;
            this.lifeSpan = lifeSpan;
            this.animated = animated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets origin of the image
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
        }

        /// <summary>
        /// Gets size of the image
        /// </summary>
        public Point Size
        {
            get { return size; }
        }

        /// <summary>
        /// Gets both dimension of the image
        /// </summary>
        public Point Dimensions
        {
            get { return dimensions; }
        }

        /// <summary>
        /// Gets the radius of the image tile
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }

        /// <summary>
        /// Gets the lifespan of the image
        /// </summary>
        public int LifeSpan
        {
            get { return lifeSpan; }
        }

        /// <summary>
        /// Gets the lifespan of the image
        /// </summary>
        public bool Animated
        {
            get { return animated; }
        }

        #endregion
    }
}
