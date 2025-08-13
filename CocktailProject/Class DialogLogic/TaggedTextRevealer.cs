using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

public class TaggedTextRevealer
{
    private class Segment
    {
        public string OpenTag;
        public string Text;
        public string CloseTag;
    }

    private List<Segment> segments;
    private int visibleCount;
    private bool isRunning;
    private double charDelay;
    private double timer;

    public TaggedTextRevealer(string text, double delayPerChar = 0.05)
    {
        segments = ParseSegments(text);
        visibleCount = 0;
        isRunning = false;
        charDelay = delayPerChar; // seconds per character
        timer = 0;
    }

    private List<Segment> ParseSegments(string input)
    {
        var segments = new List<Segment>();

        // Match {{tag}} ... {{tag}}
        var regex = new Regex(@"(\{\{[^}]+\}\})(.*?)(\{\{[^}]+\}\})", RegexOptions.Singleline);
        int lastIndex = 0;

        foreach (Match match in regex.Matches(input))
        {
            // Plain text before tag
            if (match.Index > lastIndex)
            {
                segments.Add(new Segment
                {
                    OpenTag = "",
                    Text = input.Substring(lastIndex, match.Index - lastIndex),
                    CloseTag = ""
                });
            }

            // Tagged segment
            segments.Add(new Segment
            {
                OpenTag = match.Groups[1].Value,
                Text = match.Groups[2].Value,
                CloseTag = match.Groups[3].Value
            });

            lastIndex = match.Index + match.Length;
        }

        // Any leftover plain text
        if (lastIndex < input.Length)
        {
            segments.Add(new Segment
            {
                OpenTag = "",
                Text = input.Substring(lastIndex),
                CloseTag = ""
            });
        }

        return segments;
    }

    public void Start() => isRunning = true;
    public void Stop() => isRunning = false;

    public void Reset()
    {
        visibleCount = 0;
        timer = 0;
        isRunning = false;
    }

    public void Skip()
    {
        visibleCount = GetTotalVisibleChars();
        timer = 0;
        isRunning = false;
    }

    public void Update(GameTime gameTime)
    {
        if (!isRunning || IsFinished())
            return;

        timer += gameTime.ElapsedGameTime.TotalSeconds;

        while (timer >= charDelay && !IsFinished())
        {
            visibleCount++;
            timer -= charDelay;
        }
    }

    public string GetVisibleText()
    {
        var sb = new StringBuilder();
        int remaining = visibleCount;

        foreach (var seg in segments)
        {
            if (remaining >= seg.Text.Length)
            {
                // Full segment with tags
                sb.Append(seg.OpenTag);
                sb.Append(seg.Text);
                sb.Append(seg.CloseTag);
                remaining -= seg.Text.Length;
            }
            else if (remaining > 0)
            {
                // Partial segment without tags
                sb.Append(seg.Text.Substring(0, remaining));
                remaining = 0;
            }
            else
            {
                break;
            }
        }

        return sb.ToString();
    }

    public bool IsFinished() => visibleCount >= GetTotalVisibleChars();

    private int GetTotalVisibleChars()
    {
        int totalChars = 0;
        foreach (var seg in segments)
            totalChars += seg.Text.Length;
        return totalChars;
    }
}
