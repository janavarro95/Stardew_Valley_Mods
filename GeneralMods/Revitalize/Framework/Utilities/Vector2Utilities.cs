using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omegasis.Revitalize.Framework.Utilities
{
    public static class Vector2Utilities
    {
        public static double Magnitude(this Vector2 vec)
        {
            return Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2));
        }

        /// <summary>
        /// Returns the unit vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector2 UnitVector(this Vector2 vec)
        {
            if (vec == Vector2.Zero) return Vector2.Zero;
            double mag = vec.Magnitude();
            if (mag == 0) return Vector2.Zero;
            return new Vector2((float)(vec.X / mag), (float)(vec.Y / mag));

        }

    }
}
