using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using SimpleSoundManager.Framework;
using StardewModdingAPI;

namespace SimpleSoundManager
{
    class WavSound : Sound
    {

        /// <summary>Used to actually play the song.</summary>
        DynamicSoundEffectInstance dynamicSound;

        /// <summary>Used to keep track of where in the song we are.</summary>
        int position;

        int count;

        /// <summary>Used to store the info for the song.</summary>
        byte[] byteArray;

        public string path;

        public string soundName;

        public bool loop;

        /// <summary>Get a raw disk path to the wav file.</summary>
        public WavSound(string name, string pathToWavFile, bool loop = false)
        {
            this.path = pathToWavFile;
            this.LoadWavFromFileToStream();
            this.soundName = name;
            this.loop = loop;
        }

        /// <summary>A constructor that takes a mod helper and a relative path to a wav file.</summary>
        public WavSound(IModHelper modHelper, string name, string relativePath, bool loop = false)
        {
            string path = Path.Combine(modHelper.DirectoryPath, relativePath);
            this.path = path;
            this.soundName = name;
            this.loop = loop;
        }

        /// <summary>Constructor that is more flexible than typing an absolute path.</summary>
        /// <param name="modHelper">The mod helper for the mod you wish to use to load the music files from.</param>
        /// <param name="pathPieces">The list of folders and files that make up a complete path.</param>
        public WavSound(IModHelper modHelper, string soundName, List<string> pathPieces, bool loop = false)
        {
            string dirPath = modHelper.DirectoryPath;
            foreach (string str in pathPieces)
                dirPath = Path.Combine(dirPath, str);
            this.path = dirPath;
            this.soundName = soundName;
            this.loop = loop;
        }

        /// <summary>Loads the .wav file from disk and plays it.</summary>
        public void LoadWavFromFileToStream()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            string file = this.path;
            Stream waveFileStream = File.OpenRead(file); //TitleContainer.OpenStream(file);

            BinaryReader reader = new BinaryReader(waveFileStream);

            int chunkID = reader.ReadInt32();
            int fileSize = reader.ReadInt32();
            int riffType = reader.ReadInt32();
            int fmtID = reader.ReadInt32();
            int fmtSize = reader.ReadInt32();
            int fmtCode = reader.ReadInt16();
            int channels = reader.ReadInt16();
            int sampleRate = reader.ReadInt32();
            int fmtAvgBPS = reader.ReadInt32();
            int fmtBlockAlign = reader.ReadInt16();
            int bitDepth = reader.ReadInt16();

            if (fmtSize == 18)
            {
                // Read any extra values
                int fmtExtraSize = reader.ReadInt16();
                reader.ReadBytes(fmtExtraSize);
            }

            int dataID = reader.ReadInt32();
            int dataSize = reader.ReadInt32();

            this.byteArray = reader.ReadBytes(dataSize);


            this.dynamicSound = new DynamicSoundEffectInstance(sampleRate, (AudioChannels)channels);
            this.count = this.byteArray.Length;//dynamicSound.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(1000));

            this.dynamicSound.BufferNeeded += this.DynamicSound_BufferNeeded;
        }

        void DynamicSound_BufferNeeded(object sender, EventArgs e)
        {
            try
            {
                this.dynamicSound.SubmitBuffer(this.byteArray, this.position, this.count);
            }
            catch { }

            this.position += this.count;
            if (this.position + this.count > this.byteArray.Length)
            {

                if (this.loop)
                    this.position = 0;
                //else
                //    this.stop();
            }
        }

        /// <summary>Used to pause the current song.</summary>
        public void pause()
        {
            this.dynamicSound?.Pause();
        }

        /// <summary>Used to play a song.</summary>
        public void play()
        {
            if (this.isPlaying())
                return;

            this.LoadWavFromFileToStream();
            this.dynamicSound.Play();
        }

        /// <summary>Used to play a song.</summary>
        /// <param name="volume">How lound the sound is when playing. 0~1.0f</param>
        public void play(float volume)
        {
            if (this.isPlaying())
                return;

            this.LoadWavFromFileToStream();
            this.dynamicSound.Volume = volume;
            this.dynamicSound.Play();
        }


        /// <summary>Used to resume the currently playing song.</summary>
        public void resume()
        {
            this.dynamicSound?.Resume();
        }

        /// <summary>Used to stop the currently playing song.</summary>
        public void stop()
        {
            if (this.dynamicSound != null)
            {
                this.dynamicSound.Stop(true);
                this.dynamicSound.BufferNeeded -= this.DynamicSound_BufferNeeded;
                this.position = 0;
                this.count = 0;
                this.byteArray = new byte[0];
            }
        }

        /// <summary>Used to change from one playing song to another;</summary>
        public void swap(string pathToNewWavFile)
        {
            this.stop();
            this.path = pathToNewWavFile;
            this.play();
        }

        /// <summary>Checks if the song is currently playing.</summary>
        public bool isPlaying()
        {
            return this.dynamicSound?.State == SoundState.Playing;
        }

        /// <summary>Checks if the song is currently paused.</summary>
        public bool isPaused()
        {
            return this.dynamicSound?.State == SoundState.Paused;
        }

        /// <summary>Checks if the song is currently stopped.</summary>
        public bool isStopped()
        {
            return this.dynamicSound?.State == SoundState.Stopped;
        }

        public Sound clone()
        {
            return new WavSound(this.getSoundName(), this.path);
        }

        public string getSoundName()
        {
            return this.soundName;
        }

        public void restart()
        {
            this.stop();
            this.play();
        }
    }
}
