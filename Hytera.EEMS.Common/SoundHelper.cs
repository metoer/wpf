using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Hytera.EEMS.Common
{
    public class SoundHelper
    {
        string _Sound;
        bool _IsLoop;
        MediaPlayer _player = new MediaPlayer();

        public SoundHelper(string soundFile, bool isLoop)
        {
            if (!System.IO.File.Exists(soundFile))
                return;

            // 值为 -1 表示 100% 的左侧扬声器，而值为 1 表示 100% 的右侧扬声器。
            _player.Balance = 0;
            _Sound = soundFile;
            _IsLoop = isLoop;
            _player.Volume = 1d;
            _player.Open(new Uri(soundFile , UriKind.RelativeOrAbsolute));
            if (_IsLoop)
            {
                _player.MediaEnded += new EventHandler(_player_MediaEnded);
            }

        }

        public void SetBalance(double balance) {
            this._player.Balance = balance;
        }

        public void Play()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(delegate()
                {
                    _player.Stop();
                    _player.Play();
                }));
            }
            catch (Exception ex)
            {
                
            }

        }
 
        void _player_MediaEnded(object sender , EventArgs e)
        {
            Play();

        }
        public void Stop()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(delegate()
                {
                    _player.Stop();
                }));
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
