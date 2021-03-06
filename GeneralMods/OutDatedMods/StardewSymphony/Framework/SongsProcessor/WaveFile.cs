using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region File Description
//-----------------------------------------------------------------------------
// WaveFile.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion


namespace StardewSymphony.Framework.SongsProcessor
{
    public class WaveFile
    {
        public int SampleRate { get; private set; }
        public int BitDepth { get; private set; }
        public int Channels { get; private set; }
        public byte[] Data { get; private set; }

        private WaveFile() { }

        public static WaveFile Parse(System.IO.Stream waveFileStream)
        {
            BinaryReader reader = new BinaryReader(waveFileStream);
            int chunkID = reader.ReadInt32();
            int fileSize = reader.ReadInt32();
            int riffType = reader.ReadInt32();
            int fmtID = reader.ReadInt32();
            int fmtSize = reader.ReadInt32();
            int fmtCode = reader.ReadInt16();
            int fmtChannels = reader.ReadInt16();
            int fmtSampleRate = reader.ReadInt32();
            int fmtAvgBPS = reader.ReadInt32();
            int fmtBlockAlign = reader.ReadInt16();
            int fmtBitDepth = reader.ReadInt16();

            if (fmtSize == 18)
            {
                // Read any extra values
                int fmtExtraSize = reader.ReadInt16();
                reader.ReadBytes(fmtExtraSize);
            }

            int dataID = reader.ReadInt32();
            int dataSize = reader.ReadInt32();
            byte[] dataBytes = reader.ReadBytes(dataSize);

            WaveFile parsedFile = new WaveFile()
            {
                SampleRate = fmtSampleRate,
                BitDepth = fmtBitDepth,
                Channels = fmtChannels,
                Data = dataBytes
            };

            return parsedFile;
        }
    }
}
