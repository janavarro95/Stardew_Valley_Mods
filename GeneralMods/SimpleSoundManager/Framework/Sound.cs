namespace SimpleSoundManager.Framework
{
    /// <summary>Interface used for common sound functionality;</summary>
    public interface Sound
    {
        /// <summary>Handles playing a sound.</summary>
        void play();

        void play(float volume);

        /// <summary>Handles pausing a song.</summary>
        void pause();

        /// <summary>Handles stopping a song.</summary>
        void stop();

        /// <summary>Handles restarting a song.</summary>
        void restart();

        /// <summary>Handles getting a clone of the song.</summary>
        Sound clone();

        string getSoundName();

        bool isStopped();
    }
}
