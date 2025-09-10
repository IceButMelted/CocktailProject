using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics
{
    public class AnimatedSprite : Sprite
    {
        private int _currentFrame;
        private TimeSpan _elapsed;
        private Animation _animation;
        private bool _isPlaying = true;

        /// <summary>
        /// Gets or Sets the animation for this animated sprite.
        /// </summary>
        public Animation Animation
        {
            get => _animation;
            set => SetAnimation(value, true);
        }

        /// <summary>
        /// Whether the animation is currently playing.
        /// </summary>
        public bool IsPlaying => _isPlaying;

        /// <summary>
        /// Total number of frames in the animation.
        /// </summary>
        public int FrameCount => _animation?.Frames.Count ?? 0;

        /// <summary>
        /// Creates a new animated sprite.
        /// </summary>
        public AnimatedSprite() { }

        /// <summary>
        /// Creates a new animated sprite with the specified animation.
        /// </summary>
        public AnimatedSprite(Animation animation)
        {
            SetAnimation(animation);
        }

        // -------------------
        // CONTROL METHODS
        // -------------------

        public void Play() => _isPlaying = true;
        public void Pause() => _isPlaying = false;

        public void Stop()
        {
            _isPlaying = false;
            Reset();
        }

        public void Reset()
        {
            _currentFrame = 0;
            _elapsed = TimeSpan.Zero;
            if (_animation != null && _animation.Frames.Count > 0)
                Region = _animation.Frames[0];
        }

        public void SetAnimation(Animation animation, bool reset = true)
        {
            _animation = animation;
            if (reset) Reset();
        }

        public void GoToFrame(int frameIndex)
        {
            if (_animation == null || _animation.Frames.Count == 0)
                return;

            _currentFrame = Math.Clamp(frameIndex, 0, _animation.Frames.Count - 1);
            Region = _animation.Frames[_currentFrame];
        }

        public int GetCurrentFrame() => _currentFrame;

        // -------------------
        // UPDATE METHODS
        // -------------------

        public void Update(GameTime gameTime)
        {
            if (!_isPlaying || _animation == null) return;

            _elapsed += gameTime.ElapsedGameTime;

            if (_elapsed >= _animation.Delay)
            {
                _elapsed -= _animation.Delay;
                _currentFrame = (_currentFrame + 1) % _animation.Frames.Count;
                Region = _animation.Frames[_currentFrame];
            }
        }

        public void Update(GameTime gameTime, int SpecificFrame, bool IsFreezeFrame = false, bool IsReverse = false)
        {
            if (!_isPlaying || _animation == null) return;

            _elapsed += gameTime.ElapsedGameTime;

            if (_elapsed >= _animation.Delay)
            {
                _elapsed -= _animation.Delay;

                if (IsFreezeFrame)
                    _currentFrame = SpecificFrame;
                else if (IsReverse)
                    _currentFrame--;
                else
                    _currentFrame++;

                if (_currentFrame >= _animation.Frames.Count) _currentFrame = 0;
                if (_currentFrame < 0) _currentFrame = _animation.Frames.Count - 1;

                Region = _animation.Frames[_currentFrame];
            }
        }

        public void Update(GameTime gameTime, int SpecificFrame, bool IsFreezeFrame = false, bool IsReverse = false, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            if (!_isPlaying || _animation == null) return;

            _elapsed += gameTime.ElapsedGameTime;

            if (_elapsed >= _animation.Delay)
            {
                _elapsed -= _animation.Delay;

                if (IsFreezeFrame)
                    _currentFrame = SpecificFrame;
                else if (IsReverse)
                    _currentFrame--;
                else
                    _currentFrame++;

                if (_currentFrame >= _animation.Frames.Count) _currentFrame = 0;
                if (_currentFrame < 0) _currentFrame = _animation.Frames.Count - 1;

                Region = _animation.Frames[_currentFrame];
                Effects = spriteEffects;
            }
        }

        public bool IsLastFrame()
        {
            return _currentFrame == (_animation?.Frames.Count ?? 1) - 1;
        }
    }
}
