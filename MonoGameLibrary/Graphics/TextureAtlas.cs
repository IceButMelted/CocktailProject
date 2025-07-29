﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class TextureAtlas
{
    // Support multiple textures
    private Dictionary<string, Texture2D> _textures;

    /// Store texture regions added to this atlas.
    private Dictionary<string, TextureRegion> _regions;

    // Stores animations added to this atlas.
    private Dictionary<string, Animation> _animations;

    /// <summary>
    /// Gets or Sets the source texture represented by this texture atlas.
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// Creates a new texture atlas.
    /// </summary>
    public TextureAtlas()
    {
        _textures = new Dictionary<string, Texture2D>();
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    /// <summary>
    /// Adds a texture to this texture atlas with the specified name.
    /// </summary>
    /// <param name="name">Name of Texture will be add.</param>
    /// <param name="texture">Texture2D that will be store</param>
    public void AddTexture(string name, Texture2D texture)
    {
        _textures[name] = texture;
    }

    /// <summary>
    /// Creates a new texture atlas instance using the given texture.
    /// </summary>
    /// <param name="texture">The source texture represented by the texture atlas.</param>
    public TextureAtlas(Texture2D texture)
    {
        Texture = texture;
        _regions = new Dictionary<string, TextureRegion>();  
        _animations = new Dictionary<string, Animation>();
    }

    /// <summary>
    /// Creates a new region and adds it to this texture atlas.
    /// </summary>
    /// <param name="name">The name to give the texture region.</param>
    /// <param name="x">The top-left x-coordinate position of the region boundary relative to the top-left corner of the source texture boundary.</param>
    /// <param name="y">The top-left y-coordinate position of the region boundary relative to the top-left corner of the source texture boundary.</param>
    /// <param name="width">The width, in pixels, of the region.</param>
    /// <param name="height">The height, in pixels, of the region.</param>
    public void AddRegion(string name, string textureName, int x, int y, int width, int height)
    {
        if (!_textures.ContainsKey(textureName))
            throw new Exception($"Texture '{textureName}' not found.");

        TextureRegion region = new TextureRegion(_textures[textureName], x, y, width, height);
        _regions[name] = region;
    }

    /// <summary>
    /// Creates a new sprite using the region from this texture atlas with the specified name.
    /// </summary>
    /// <param name="regionName">The name of the region to create the sprite with.</param>
    /// <returns>A new Sprite using the texture region with the specified name.</returns>
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        return new Sprite(region);
    }

    /// <summary>
    /// Gets the region from this texture atlas with the specified name.
    /// </summary>
    /// <param name="name">The name of the region to retrieve.</param>
    /// <returns>The TextureRegion with the specified name.</returns>
    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }

    /// <summary>
    /// Gets the texture2D from the region of this texture atlas by name.
    /// </summary>
    /// <returns></returns>
    public Texture2D GetTexture2D(string name)
    {
        if (_regions.TryGetValue(name, out TextureRegion region))
        {
            return region.GetTexture2D();
        }
        throw new KeyNotFoundException($"Region '{name}' not found in the texture atlas.");
    }

    /// <summary>
    /// Removes the region from this texture atlas with the specified name.
    /// </summary>
    /// <param name="name">The name of the region to remove.</param>
    /// <returns></returns>
    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }

    /// <summary>
    /// Removes all regions from this texture atlas.
    /// </summary>
    public void Clear()
    {
        _regions.Clear();
    }


    /// <summary>
    /// Creates a new texture atlas based a texture atlas xml configuration file.
    /// </summary>
    /// <param name="content">The content manager used to load the texture for the atlas.</param>
    /// <param name="fileName">The path to the xml file, relative to the content root directory..</param>
    /// <returns>The texture atlas created by this method.</returns>
    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        TextureAtlas atlas = new TextureAtlas();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;
                #region texture
                // Load all textures
                // The <Textures> element contains individual <Texture> elements, each one describing
                // a different texture region within the atlas.  
                //
                // Example:
                // <Textures>
                //   <Texture name="atlas">Images/atlas</Texture>
                //   <Texture name="atlas2">Images/atlas2</Texture>
                // </Textures>
                //
                // So we retrieve all of the <Texture> elements then loop through each one
                // to get path of Texture.
                var textureElements = root.Element("Textures")?.Elements("Texture");
                if (textureElements != null)
                {
                    foreach (var texElement in textureElements)
                    {
                        string name = texElement.Attribute("name")?.Value;
                        string path = texElement.Value;
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path))
                        {
                            atlas.AddTexture(name, content.Load<Texture2D>(path));
                        }
                    }
                }
                #endregion

                #region region
                // Load regions
                // The <Regions> element contains individual <Region> elements, each one describing
                // a different texture region within the atlas.  
                //
                // Example:
                // <Regions>
                //      <Region name="spriteOne" name="Atlas1" x="0" y="0" width="32" height="32" />
                //      <Region name="spriteTwo" name="Atlas2" x="32" y="0" width="32" height="32" />
                // </Regions>
                //
                // So we retrieve all of the <Region> elements then loop through each one
                // and generate a new TextureRegion instance from it and add it to this atlas.
                var regions = root.Element("Regions")?.Elements("Region");
                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        string name = region.Attribute("name")?.Value;
                        string textureName = region.Attribute("texture")?.Value;
                        int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                        int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                        int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                        int height = int.Parse(region.Attribute("height")?.Value ?? "0");

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(textureName))
                        {
                            atlas.AddRegion(name, textureName, x, y, width, height);
                        }
                    }
                }
#endregion

                #region animation
                // Load animations
                // The <Animations> element contains individual <Animation> elements, each one describing
                // a different animation within the atlas.
                //
                // Example:
                // <Animations>
                //      <Animation name="animation" delay="100">
                //          <Frame region="spriteOne" />
                //          <Frame region="spriteTwo" />
                //      </Animation>
                // </Animations>
                //
                // So we retrieve all of the <Animation> elements then loop through each one
                // and generate a new Animation instance from it and add it to this atlas.
                var animationElements = root.Element("Animations")?.Elements("Animation");
                if (animationElements != null)
                {
                    foreach (var animationElement in animationElements)
                    {
                        string name = animationElement.Attribute("name")?.Value;
                        float delayMs = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                        TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);

                        List<TextureRegion> frames = new List<TextureRegion>();
                        var frameElements = animationElement.Elements("Frame");

                        foreach (var frame in frameElements)
                        {
                            string regionName = frame.Attribute("region")?.Value;
                            TextureRegion region = atlas.GetRegion(regionName);
                            frames.Add(region);
                        }

                        atlas.AddAnimation(name, new Animation(frames, delay));
                    }
                }
                #endregion
            }
        }

        return atlas;
    }



    /// <summary>
    /// Adds the given animation to this texture atlas with the specified name.
    /// </summary>
    /// <param name="animationName">The name of the animation to add.</param>
    /// <param name="animation">The animation to add.</param>
    public void AddAnimation(string animationName, Animation animation)
    {
        _animations.Add(animationName, animation);
    }

    /// <summary>
    /// Gets the animation from this texture atlas with the specified name.
    /// </summary>
    /// <param name="animationName">The name of the animation to retrieve.</param>
    /// <returns>The animation with the specified name.</returns>
    public Animation GetAnimation(string animationName)
    {
        return _animations[animationName];
    }

    /// <summary>
    /// Removes the animation with the specified name from this texture atlas.
    /// </summary>
    /// <param name="animationName">The name of the animation to remove.</param>
    /// <returns>true if the animation is removed successfully; otherwise, false.</returns>
    public bool RemoveAnimation(string animationName)
    {
        return _animations.Remove(animationName);
    }

    /// <summary>
    /// Creates a new animated sprite using the animation from this texture atlas with the specified name.
    /// </summary>
    /// <param name="animationName">The name of the animation to use.</param>
    /// <returns>A new AnimatedSprite using the animation with the specified name.</returns>
    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        Animation animation = GetAnimation(animationName);
        return new AnimatedSprite(animation);
    }





}
