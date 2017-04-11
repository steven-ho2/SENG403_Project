using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Team4Clock_Shared
{
    public interface IAudioPlayerService
    {
        void Play(string pathToAudioFile);
        void Play();
        void Pause();
        Action OnFinishedPlaying { get; set; }
    }
}
