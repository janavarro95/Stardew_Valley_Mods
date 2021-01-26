using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardustCore.StardustMath
{
    /// <summary>A Class that helps represents 32 bit hex.</summary>
    class Hex32 : Hex
    {
        /// <summary>A default constructor.</summary>
        public Hex32()
        {
            this.hexValue = "0x00000000";
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="hexValue">A string in hex representation. Ex) 0x00000000</param>
        public Hex32(string hexValue)
        {
            this.hexValue = hexValue;
            if (!this.verifyHexLength())
                this.hexValue = "0x00000000";
        }

        /// <summary>Construct an instance.</summary>
        /// <param name="value">An int to be converted into Hex.</param>
        public Hex32(int value)
        {
            this.hexValue = value.ToString("X");
            if (!this.verifyHexLength())
                this.hexValue = "0x00000000";
        }

        /// <summary>Makes sure the hex value is the appropriate length.</summary>
        public override bool verifyHexLength()
        {
            if (this.hexValue.Length != 10) return false;
            else return true;
        }

        /// <summary>Converts a hex value to a string.</summary>
        public Color toColor()
        {
            var bytes = this.getBytes();
            int red = this.convertHexByteTo255Int(bytes[0]);
            int green = this.convertHexByteTo255Int(bytes[1]);
            int blue = this.convertHexByteTo255Int(bytes[2]);
            int alpha = this.convertHexByteTo255Int(bytes[3]);
            return new Color(red, green, blue, alpha);
        }

        /// <summary>Get the individual byte strings associated with this hex value.</summary>
        public override List<string> getBytes()
        {
            List<string> bytes = new List<string>();
            bytes.Add(this.getByte(0));
            bytes.Add(this.getByte(1));
            bytes.Add(this.getByte(2));
            bytes.Add(this.getByte(3));
            return bytes;
        }
    }
}
