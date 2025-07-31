using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace Lab05_01.Graphics;

public class AnimatedSprite : Sprite
{
    private int _currentFrame;
    private TimeSpan _elapsed;
    private Animation _animation;

    /// <summary>
    /// Gets or Sets the animation for this animated sprite.
    /// </summary>
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Region = _animation.Frames[0];
        }
    }

    /// <summary>
    /// Creates a new animated sprite.
    /// </summary>
    public AnimatedSprite() { }

    /// <summary>
    /// Creates a new animated sprite with the specified frames and delay.
    /// </summary>
    /// <param name="animation">The animation for this animated sprite.</param>
    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }

    /// <summary>
    /// Updates this animated sprite.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game timing values provided by the framework.</param>
    public void Update(GameTime gameTime)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }

            Region = _animation.Frames[_currentFrame];
        }
    }

    /// <summary>
    ///  Update with Freez specific frame is true or Loop roll back animation is true.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game timing values provided by the framework.</param>
    public void Update(GameTime gameTime, int SpecificFrame, bool IsFreezeFramem = false, bool IsReverse = false)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            if (IsFreezeFramem)
                _currentFrame = SpecificFrame;
            else if (IsReverse)
                _currentFrame--;
            else
                _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }
            if (_currentFrame < 0)
            {
                _currentFrame = _animation.Frames.Count - 1;
            }

            Region = _animation.Frames[_currentFrame];
        }
    }

    /// <summary>
    ///  Update with Freez specific frame is true or Loop roll back animation is true and flip or not.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game timing values provided by the framework.</param>
    public void Update(GameTime gameTime, int SpecificFrame, bool IsFreezeFramem = false, bool IsReverse = false, SpriteEffects spriteEffects = SpriteEffects.None)
    {
        _elapsed += gameTime.ElapsedGameTime;
        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            if (IsFreezeFramem)
                _currentFrame = SpecificFrame;
            else if (IsReverse)
                _currentFrame--;
            else
                _currentFrame++;
            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }
            if (_currentFrame < 0)
            {
                _currentFrame = _animation.Frames.Count - 1;
            }
            Region = _animation.Frames[_currentFrame];
            Effects = spriteEffects;
        }
    }


    public bool IsLastFrame()
    {
        return _currentFrame == _animation.Frames.Count - 1;
    }


}
