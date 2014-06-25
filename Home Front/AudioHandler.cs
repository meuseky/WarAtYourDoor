using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Home_Front
{
    //Handles all calls to play (& stop) music & SFX
    //Error checking code for machines with no audio devices (College computers)
    static class AudioHandler
    {
        #region Variables
        static public Song[] songs;
        static public SoundEffect[] sfxs;
        //Holds SFx instances
        static public List<SoundEffectInstance> sfxInstance;
        #endregion

        #region Methods
        //Constructor
        static AudioHandler()
        {
            sfxInstance = new List<SoundEffectInstance>();
            MediaPlayer.Volume = 0.25f;
        }

        //Plays uninstanced SFX
        public static void PlayEffect(int effectNumber)
        {
            try
            {
                //sfxInstance[effectNumber].Play();
                sfxs[effectNumber].Play();
            }
            catch (NoAudioHardwareException)
            {
            }
            catch (NullReferenceException)
            {
            }
        }

        //Plays instanced SFX
        public static void PlayEffectInstance(int effectNumber)
        {
            try
            {
                sfxInstance[effectNumber].Play();
            }
            catch (NoAudioHardwareException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }

        }

        //Ends SFX playing
        public static void StopEffectInstance(int effectNumber)
        {
            try
            {
                sfxInstance[effectNumber].Stop();
            }
            catch (NoAudioHardwareException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        //Starts level music
        public static void PlaySong(int songNumber)
        {
            try
            {
                MediaPlayer.Play(songs[songNumber]);
            }
            catch (NoAudioHardwareException)
            {
            }
            catch (ArgumentNullException)
            {
            }

        }

        //Ends level music
        public static void StopSong(int songNumber)
        {
            try
            {
                MediaPlayer.Stop();
            }
            catch (NoAudioHardwareException)
            {
            }
        }

        //Creates SFX instances to be played
        public static void CreateEffectInstances()
        {
            try
            {
                foreach (SoundEffect sfx in sfxs)
                {
                    sfxInstance.Add(sfx.CreateInstance());
                }
                sfxInstance[3].IsLooped = true;
                MediaPlayer.IsRepeating = true;
            }
            catch (NullReferenceException)
            {
            }
        }
        #endregion                
    }
}
