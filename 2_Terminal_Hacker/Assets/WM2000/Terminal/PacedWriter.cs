using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PacedWriter : MonoBehaviour
{

    static PacedWriter writer;

    // Writing pacing
    private const float defaultPacing = 0.02f;
    private const float defaultDelay = 0.3f;

    private Queue<PacedContent> contents;
    private PacedContent currentContent;

    float timeSinceLastWrite;
    float currentWait;

    public static bool usePacing = true;

    private void Awake()
    {
        if (writer == null) { writer = this; } // Be the one
        contents = new Queue<PacedContent>();
    }

    // Use this for initialization
    void Start()
    {
        timeSinceLastWrite = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!usePacing)
        {
            return;
        }

        SetCurrentContent();

        if (currentContent != null)
        {
            Terminal.DisableInput();
            if (timeSinceLastWrite >= currentWait)
            {
                string nextChar = currentContent.NextChar();
                if (nextChar == "")
                {
                    currentContent = null;
                }
                else
                {
                    Terminal.WriteChar(nextChar);
                }

                currentWait = currentContent.pacing;
                timeSinceLastWrite = 0;

                return;
            }
        }
        else
        {
            Terminal.EnableInput();
        }

        timeSinceLastWrite += Time.deltaTime;
    }

    public static void WriteLine(string content, float delay, float pacing)
    {
        if (!usePacing)
        {
            Terminal.WriteLine(content);
        }
        else
        {
            string[] lines = content.Split('\n');
            foreach (string line in lines)
            {
                writer.contents.Enqueue(new PacedContent(line + '\n', delay, pacing));
            }   
        }
    }

    public static void WriteLine(string content)
    {
        WriteLine(content, defaultDelay, defaultPacing);
    }

    public static void WriteDelayedLine(string content, float delay)
    {
        WriteLine(content, delay, defaultPacing);
    }

    public static void WritePacedLine(string content, float pacing)
    {
        WriteLine(content, defaultDelay, pacing);
    }

    private void SetCurrentContent()
    {
        if (currentContent == null)
        {
            if (contents.Count > 0)
            {
                currentContent = contents.Dequeue();
                currentWait = currentContent.delay;
            }
        }
    }
}


class PacedContent
{
    public string content;
    public readonly float delay;
    public readonly float pacing;

    public PacedContent(string content, float delay, float pacing)
    {
        this.content = content;
        this.delay = delay;
        this.pacing = pacing;
    }

    public string NextChar()
    {
        if (content == "")
        {
            return "";
        }
        string charToWrite = content.Substring(0, 1);
        content = content.Substring(1);
        return charToWrite;
    }
}
