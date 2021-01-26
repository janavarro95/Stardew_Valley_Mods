using System;
using System.Collections.Generic;

namespace StardustCore.StardustMath
{
    /// <summary>Base class for hex representation.</summary>
    public class Hex
    {
        /// <summary>The hex value represented as a string.</summary>
        public string hexValue;

        /// <summary>Default constructor.</summary>
        public Hex() { }

        /// <summary>Construct an instance.</summary>
        /// <param name="HexValue"></param>
        public Hex(string HexValue) { }

        /// <summary>Verifies that the hex is of the specified length.</summary>
        public virtual bool verifyHexLength()
        {
            return true;
        }

        /// <summary>Trims the hex value by removing the leading 0x;</summary>
        public virtual string trimHex()
        {
            return this.hexValue.Split('x')[1];
        }

        /// <summary>A virtual function to be overriden.</summary>
        public virtual List<string> getBytes()
        {
            return new List<string>();
        }

        /// <summary>Converts a Hex byte (represented as a length two string) to an int equal or less than 255. Ex ff=255;</summary>
        /// <param name="value">The length two value to be converted.</param>
        public virtual int convertHexByteTo255Int(string value)
        {
            int val1 = this.convertHexValueToInt(value[0]);
            int val2 = this.convertHexValueToInt(value[1]);
            val1 *= 16;
            val2 *= 16;
            return val1 + val2;
        }

        /// <summary>Converts a hex char to an int.</summary>
        /// <param name="c"></param>
        public virtual int convertHexValueToInt(char c)
        {
            if (c == 'a') return 10;
            else if (c == 'b') return 11;
            else if (c == 'c') return 12;
            else if (c == 'd') return 13;
            else if (c == 'e') return 14;
            else if (c == 'f') return 15;
            else return Convert.ToInt32(c);
        }

        /// <summary>Gets the associated byte from the hex positioning.</summary>
        /// <param name="index"></param>
        public virtual string getByte(int index)
        {
            string hex = this.trimHex();
            int val = index * 2 + 1;
            char str1 = hex[index * 2];
            char str2 = hex[val];
            return (str1.ToString() + str2.ToString());
        }
    }
}
