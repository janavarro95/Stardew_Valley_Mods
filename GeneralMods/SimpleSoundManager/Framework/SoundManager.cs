using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;

namespace SimpleSoundManager.Framework
{
    public class SoundManager
    {
        public Dictionary<string, Sound> sounds;
        public Dictionary<string, XACTMusicPair> musicBanks;

        public float volume;

        public List<Sound> currentlyPlayingSounds = new List<Sound>();

        /// <summary>Constructor for this class.</summary>
        public SoundManager()
        {
            this.sounds = new Dictionary<string, Sound>();
            this.musicBanks = new Dictionary<string, XACTMusicPair>();
            this.currentlyPlayingSounds = new List<Sound>();
            this.volume = 1.0f;
        }

        /// <summary>Constructor for wav files.</summary>
        public void loadWavFile(string soundName, string pathToWav)
        {
            WavSound wav = new WavSound(soundName, pathToWav);
            SimpleSoundManagerMod.ModMonitor.Log("Getting sound file:" + soundName);
            try
            {
                this.sounds.Add(soundName, wav);
            }
            catch { }
        }

        /// <summary>Constructor for wav files.</summary>
        public void loadWavFile(IModHelper helper, string soundName, string relativePath)
        {
            WavSound wav = new WavSound(helper, soundName, relativePath);
            SimpleSoundManagerMod.ModMonitor.Log("Getting sound file:" + soundName);
            try
            {
                this.sounds.Add(soundName, wav);
            }
            catch
            {
                //Sound already added so no need to worry?
            }
        }

        /// <summary>Constructor for wav files.</summary>
        public void loadWavFile(IModHelper helper, string songName, List<string> pathToWav)
        {
            WavSound wav = new WavSound(helper, songName, pathToWav);
            SimpleSoundManagerMod.ModMonitor.Log("Getting sound file:" + songName);
            try
            {
                this.sounds.Add(songName, wav);
            }
            catch { }
        }

        /// <summary>Constructor for XACT files.</summary>
        public void loadXACTFile(WaveBank waveBank, ISoundBank soundBank, string songName)
        {
            XACTSound xactSound = new XACTSound(waveBank, soundBank, songName);
            this.sounds.Add(songName, xactSound);
        }

        /// <summary>Constructor for XACT files based on already added music packs.</summary>
        public void loadXACTFile(string pairName, string songName)
        {
            XACTMusicPair musicPair = this.getMusicPack(pairName);
            if (pairName == null)
                return;
            this.loadXACTFile(musicPair.waveBank, musicPair.soundBank, songName);
        }

        /// <summary>Creates a music pack pair that holds .xwb and .xsb music files.</summary>
        /// <param name="helper">The mod's helper that will handle the path of the files.</param>
        /// <param name="pairName">The name of this music pack pair.</param>
        /// <param name="wavName">The relative path to the .xwb file</param>
        /// <param name="soundName">The relative path to the .xsb file</param>
        public void loadXACTMusicBank(IModHelper helper, string pairName, string wavName, string soundName)
        {
            this.musicBanks.Add(pairName, new XACTMusicPair(helper, wavName, soundName));
        }

        /// <summary>Gets the music pack pair from the sound pool.</summary>
        public XACTMusicPair getMusicPack(string name)
        {
            foreach (var pack in this.musicBanks)
            {
                if (name == pack.Key)
                    return pack.Value;
            }
            return null;
        }

        /// <summary>Gets a clone of the loaded sound.</summary>
        public Sound getSoundClone(string name)
        {
            foreach (var sound in this.sounds)
            {
                if (sound.Key == name)
                    return sound.Value.clone();
            }
            return null;
        }

        /// <summary>Play the sound with the given name.</summary>
        public void playSound(string soundName)
        {
            SimpleSoundManagerMod.ModMonitor.Log("Trying to play sound: " + soundName);
            foreach (var sound in this.sounds)
            {
                if (sound.Key == soundName)
                {
                    SimpleSoundManagerMod.ModMonitor.Log("Time to play sound: " + soundName);
                    var s = this.getSoundClone(soundName);
                    s.play(this.volume);
                    this.currentlyPlayingSounds.Add(s);
                    break;
                }
            }
        }

        /// <summary>Play the sound with the given name and volume.</summary>
        public void playSound(string soundName, float volume = 1.0f)
        {
            SimpleSoundManagerMod.ModMonitor.Log("Trying to play sound: " + soundName);
            foreach (var sound in this.sounds)
            {
                if (sound.Key == soundName)
                {
                    SimpleSoundManagerMod.ModMonitor.Log("Time to play sound: " + soundName);
                    var s = this.getSoundClone(soundName);
                    s.play(volume);
                    this.currentlyPlayingSounds.Add(s);
                    break;
                }
            }
        }

        /// <summary>Stop the sound that is playing.</summary>
        public void stopSound(string soundName)
        {
            List<Sound> removalList = new List<Sound>();
            foreach (var sound in this.currentlyPlayingSounds)
            {
                if (sound.getSoundName() == soundName)
                {
                    sound.stop();
                    removalList.Add(sound);
                }
            }
            foreach (var v in removalList)
                this.currentlyPlayingSounds.Remove(v);
        }

        /// <summary>Pause the sound with this name?</summary>
        public void pauseSound(string soundName)
        {
            List<Sound> removalList = new List<Sound>();
            foreach (var sound in this.currentlyPlayingSounds)
            {
                if (sound.getSoundName() == soundName)
                {
                    sound.pause();
                    removalList.Add(sound);
                }
            }
            foreach (var v in removalList)
                this.currentlyPlayingSounds.Remove(v);
        }

        public void swapSounds(string newSong)
        {
            this.playSound(newSong);
        }

        public void update()
        {
            List<Sound> removalList = new List<Sound>();
            foreach (Sound song in this.currentlyPlayingSounds)
            {
                if (song.isStopped())
                    removalList.Add(song);
            }
            foreach (var v in removalList)
                this.currentlyPlayingSounds.Remove(v);
        }

        public void stopAllSounds()
        {
            foreach (var v in this.currentlyPlayingSounds)
                v.stop();
        }
    }
}
