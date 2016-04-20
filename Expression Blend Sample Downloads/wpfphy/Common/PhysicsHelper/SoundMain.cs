/////////////////////////////////////////////////////////////////
//
// Silverlight + FarSeer Physics Helper
//
// by Andy Beaulieu - http://www.andybeaulieu.com
//
// LICENSE: This code is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. ANY 
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS
// OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
/////////////////////////////////////////////////////////////////using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System;
#if SILVERLIGHT_PHONE
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework;
#endif 

namespace Spritehand.FarseerHelper
{
    public class SoundMain
    {
        private List<SoundElement> _sounds;
        private double _loopTime = 0;
        private double _loopIndex = 0;

#if SILVERLIGHT_PHONE
        SoundEffectInstance _effect;

        public SoundEffectInstance Effect
        {
            get
            {
                return _effect;
            }
        }
#else
        private int _currentSoundIndex = 0;
        private static int _soundId = 0;
 
#endif

        public DateTime LastPlayTime { get; set; }

        public double LoopTime
        {
            get
            {
                return _loopTime;
            }

            set
            {
                _loopTime = value;
            }
        }

        public double Volume { get; set; }

        public SoundMain(Canvas container, string soundFile, int numBuffers, double loopTime)
        {
            _sounds = new List<SoundElement>();

            LoopTime = loopTime;
            Volume = -1;

            // create the sound elements
#if SILVERLIGHT_PHONE
            // use XNA for sound - mixing is done for us
            Stream stream = TitleContainer.OpenStream(soundFile);
            SoundEffect effect = SoundEffect.FromStream(stream);
            _effect = effect.CreateInstance();
            if (loopTime > 0)
                _effect.IsLooped = true;
#else
            // use MediaElement for Sound - one for each "buffer"
            for (int i = 0; i < numBuffers; i++)
            {
                _soundId++;

                // create the sound from XAML
                SoundElement theSound = new SoundElement();
                theSound.Sound.Volume = 0;
                if (loopTime <= 0)
                    theSound.Sound.AutoPlay = true;
                else
                {
                    // set this sound up to loop

                    theSound.Sound.AutoPlay = false;


                }
                theSound.Sound.Source = new Uri(soundFile, UriKind.Relative);
                string shortName = soundFile.Substring(soundFile.LastIndexOf("/") + 1, soundFile.LastIndexOf(".") - 1 - soundFile.LastIndexOf("/"));
                //theSound.Name = "sound" + shortName + _soundId.ToString();

                // add the sound to our List
                _sounds.Add(theSound);

                theSound.Sound.MarkerReached += new TimelineMarkerRoutedEventHandler(theSound_MarkerReached);
                theSound.Sound.MediaOpened += new RoutedEventHandler(theSound_MediaOpened);
                theSound.Sound.MediaEnded += new System.Windows.RoutedEventHandler(theSound_MediaEnded);

                // add the sound to our main page
                container.Children.Add(theSound.Sound);
            }
#endif

        }

        void theSound_MarkerReached(object sender, TimelineMarkerRoutedEventArgs e)
        {

            MediaElement sound = (sender as MediaElement);
            if (LoopTime > 0)
            {
                _loopIndex = (_loopIndex + 1) % _sounds.Count;
                for (int i = 0; i < _sounds.Count; i++)
                {
                    if (i == _loopIndex)
                    {
                        _sounds[i].Sound.Position = TimeSpan.FromMilliseconds(0);
                        _sounds[i].Sound.Volume = (Volume == -1) ? 0.5 : Volume;
                        _sounds[i].Sound.Play();
                    }
                }
            }
        }

        void theSound_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (LoopTime > 0)
            {
                // set up our loop marker (this avoids gaps in loop)
                MediaElement sound = (sender as MediaElement);

                TimelineMarker marker = new TimelineMarker();
                marker.Text = "x";
                marker.Type = "y";
                marker.Time = TimeSpan.FromMilliseconds(LoopTime);

#if (!SILVERLIGHT_PHONE)
                sound.Markers.Add(marker);
#endif
            }
        }

        void theSound_MediaEnded(object sender, EventArgs e)
        {
            // restore the volume in the case of the first play
            MediaElement sound = (sender as MediaElement);

            // looping done with marker method above!
        }

        public void Play()
        {
#if (!SILVERLIGHT_PHONE)
            _sounds[_currentSoundIndex].Sound.Volume = (Volume == -1) ? 0.5 : Volume;
            _sounds[_currentSoundIndex].Sound.Stop();
            if (_sounds[_currentSoundIndex].ReadyToPlay)
            {
                _sounds[_currentSoundIndex].Sound.Play();
                _currentSoundIndex = (_currentSoundIndex + 1) % _sounds.Count;

                LastPlayTime = DateTime.Now;
            }
            else
            {
                _sounds[_currentSoundIndex].PlayOnLoad = true;
            }
#else
            _effect.Play();
#endif
        }

        public void Stop()
        {
            foreach (SoundElement sound in _sounds)
                sound.Sound.Stop();
        }

    }

    public class SoundElement 
    {
        public bool ReadyToPlay { get; set; }
        public MediaElement Sound { get; set; }
        public bool PlayOnLoad { get; set; }

        public SoundElement()
        {
            Sound = new MediaElement();
            Sound.MediaOpened += new RoutedEventHandler(Sound_MediaOpened);
        }

        void Sound_MediaOpened(object sender, RoutedEventArgs e)
        {
            ReadyToPlay = true;
            if (PlayOnLoad)
                Sound.Play();
        }

    }
}
