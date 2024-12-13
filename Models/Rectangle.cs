using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle_2.Models
{
    using System;
    using System.Runtime.InteropServices;
    using Circle_2.Utils.Models;
    using static Circle_2.Utils.MonitorHelper;

    public class Rectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="left">Left coordinate.</param>
        /// <param name="top">Top coordinate.</param>
        /// <param name="right">Right coordinate.</param>
        /// <param name="bottom">Bottom coordinate.</param>
        public Rectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Rectangle"/> from a RECT structure.
        /// </summary>
        /// <param name="monitorBounds">The RECT structure containing monitor bounds.</param>
        /// <returns>A new <see cref="Rectangle"/> instance.</returns>
        public static Rectangle CreateFromRECT(RECT monitorBounds)
        {
            return new Rectangle(monitorBounds.Left, monitorBounds.Top, monitorBounds.Right, monitorBounds.Bottom);
        }

        /// <summary>
        /// Returns the bounds as a tuple (left, top, right, bottom).
        /// </summary>
        /// <returns>A tuple representing the bounds.</returns>
        public (int Left, int Top, int Right, int Bottom) ToTuple()
        {
            return (Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Calculates the width of the bounds.
        /// </summary>
        /// <returns>The width of the bounds.</returns>
        public int Width()
        {
            return Right - Left;
        }

        /// <summary>
        /// Calculates the height of the bounds.
        /// </summary>
        /// <returns>The height of the bounds.</returns>
        public int Height()
        {
            return Bottom - Top;
        }

        /// <summary>
        /// Determines if the bounds are equal to another bounds instance.
        /// </summary>
        /// <param name="other">The other bounds to compare.</param>
        /// <returns>True if equal; otherwise, false.</returns>
        public bool Equals(Rectangle other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
        }

        /// <summary>
        /// Determines if the bounds match the specified rectangle parameters.
        /// </summary>
        /// <param name="left">The left coordinate.</param>
        /// <param name="top">The top coordinate.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns>True if equal; otherwise, false.</returns>
        public bool Equals(int left, int top, int width, int height)
        {
            return Left == left && Top == top && Right == left + width && Bottom == top + height;
        }

        /// <summary>
        /// Returns a string representation of the bounds.
        /// </summary>
        /// <returns>A string describing the bounds.</returns>
        public override string ToString()
        {
            return $"Bounds(Left: {Left}, Top: {Top}, Right: {Right}, Bottom: {Bottom})";
        }
    }
}
