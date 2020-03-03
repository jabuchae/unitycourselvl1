using System;
using System.Collections.Generic;

public class DisplayBuffer
{
    List<string> logLines = new List<string>();

    InputBuffer inputBuffer;
    const float FLASH_INTERVAL = .5f;

    public DisplayBuffer(InputBuffer inputBuffer)
    {
        this.inputBuffer = inputBuffer;
        inputBuffer.onCommandSent += OnCommand;
    }

    public void WriteLine(string line)
    {
        logLines.Add(line);
    }

    public void Clear()
    {
        logLines = new List<string>();
    }

    public string GetDisplayBuffer(float time, int width, int height)
    {
        var lines = GetAllDisplayLines(time);
        var wrappedLines = Wrap(width, lines);
        var viewportLines = CutViewport(height, wrappedLines);
        return viewportLines;
    }

    private string GetAllDisplayLines(float time)
    {
        string output = "";
        foreach (string line in logLines)
        {
            output += line + '\n';
        }
        output += inputBuffer.GetCurrentInputLine();
        output += GetFlashingCursor(time);
        return output;
    }

    private string Wrap(int width, string str)
    {
        string output = "";
        int column = 1;
        string[] lineArray = str.Split('\n');

        foreach (string line in lineArray)
        {
            string[] wordArray = line.Split(' ');
            foreach (string word in wordArray)
            {
                if (column + word.Length >= width)
                {
                    output += '\n';
                    column = 1;
                }

                output += word;
                column += word.Length;
                output += ' ';
                column += 1;
            }
            output += '\n';
            column = 1;
        }
        
        return output.Remove(output.Length-1); // Remove extra whitespace
    }

    private string CutViewport(int height, string lines)
    {
        string output = "";
        int rowCount = 1;
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            if (rowCount > height)
            {
                return output;
            }
            if (lines[i] == '\n')
            {
                rowCount++;
            }
            output = lines[i] + output;
        }
        return output;
    }

    private char GetFlashingCursor(float time)
    {
        if (time % (2 * FLASH_INTERVAL) <= FLASH_INTERVAL)
        {
            return '_';
        }
        else
        {
            return ' ';
        }
    }

    void OnCommand(string command)
    {
        logLines.Add(command);
    }
}