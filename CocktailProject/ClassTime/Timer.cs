using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.ClassTime
{
    public class Timer
    {
        private readonly float _timeLegnth;
        private string _text;
        private float _timeLeft;
        private bool _IsActive;
        public bool Repeat { get; set; }

        public Timer(float timeLength, bool repeat = false)
        {
            _timeLegnth = timeLength;
            _timeLeft = timeLength;
            Repeat = repeat;
            _IsActive = false;
        }

        private void FormatText() { 
            _text = TimeSpan.FromSeconds(_timeLeft).ToString(@"mm\:ss");
            Debug.WriteLine(_text); // For debugging purposes
        }

        public void StartStop() { 
            _IsActive = !_IsActive;
        }

        public void Reset() { 
            _timeLeft = _timeLegnth;
            FormatText();
        }

        public void UpdateTime() {
            if (!_IsActive) return;
            _timeLeft -= Time.Time_Global;
            FormatText();
        }

        public bool IsTimeUp() {
            if (_timeLeft <= 0f)
                return true;
            else
                return false;
        }
    }
}
