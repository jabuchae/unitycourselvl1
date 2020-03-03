using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    // Levels
    private Hackable[] hackables;

    // Different states of the game
    enum State { menu, hacking, hackResult, journal, ending }

    // Game State
    private State currentState;
    private string password;
    private int level;

    private List<string> journalEntries;

    private int[] levelsSolved;
    private readonly int[] maxPuzzles = { 5, 1, 4 };

    // Start is called before the first frame update
    void Start()
    {
        hackables = new Hackable[] {
            new SiblingJournal(),
            new NeighbourWifi()
        };

        journalEntries = new List<string>();
        currentState = State.menu;
        levelsSolved = new int[] {0,0,0};

        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowMainMenu()
    {
        currentState = State.menu;
        Terminal.ClearScreen();
        Terminal.WriteLine("Feb 2nd, 2007");
        Terminal.WriteLine("    Boring day isn't it?");
        Terminal.WriteLine("");
        ShowOptions();
    }

    void ShowOptions()
    {
        for (int i = 0; i < hackables.Length; i++)
        {
            if (levelsSolved[i] < maxPuzzles[i])
            {
                Terminal.WriteLine("Press " + (i + 1) + " to hack into " + hackables[i].GetName());
                Terminal.WriteLine("");
            }
        }

        ShowMenuHint();
    }

    void ShowMenuHint()
    {
        string journalAccess = levelsSolved[0] >= 1 ? ", journal" : "";
        Terminal.WriteLine("You can always type menu" + journalAccess + " or quit.");
    }

    void OnUserInput(string input)
    {
        if (currentState == State.ending)
        {
            QuitGame();
            return;
        }

        if (ProcessGeneralCommand(input)) {
            return;
        }
        switch (currentState)
        {
            case State.menu:
                ProcessMenuInput(input);
                break;
            case State.hacking:
                ProcessPasswordAttempt(input);
                break;
            case State.journal:
                ProcessJournalSelection(input);
                break;
            case State.hackResult:
                break;
            default:
                break;
        }
        
    }

    void QuitGame()
    {
        // save any game data here
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
         Application.Quit();
        #endif
    }

    void ProcessJournalSelection(string input)
    {
        foreach (string entry in journalEntries)
        {
            if (ClearFormat(input) == ClearFormat(GetFirstLine(entry)))
            {
                ShowJournalEntry(entry);
                break;
            }
        }
    }

    string ClearFormat(string text)
    {
        var clearText = text.ToLower();
        clearText = clearText.Replace("\n", "");
        clearText = clearText.Replace(" ", "");
        clearText = clearText.Replace(",", "");
        clearText = clearText.Replace("2nd", "2");
        clearText = clearText.Replace("20th", "20");
        clearText = clearText.Replace("17th", "17");
        
        return clearText;
    }

    void ShowJournalEntry(string entry)
    {
        Terminal.ClearScreen();
        Terminal.WriteLine(entry);
        Terminal.WriteLine("");
        Terminal.WriteLine("Type menu to continue or journal to see more entries.");
    }

    void ProcessMenuInput(string input)
    {
        int level = -1;
        for (int i = 0; i < hackables.Length; i++)
        {
            if (input == (i+1).ToString())
            {
                level = i;
            }
        }

        if (level == -1)
        {
            ShowLevelSelectionError();
        }
        else
        {
            this.level = level;
            ShowPuzzle();
        }
    }

    bool ProcessGeneralCommand(string input)
    {
        if (input == "menu")
        {
            ShowMainMenu();
            return true;
        }

        if (input == "journal" && levelsSolved[0] >= 1)
        {
            ShowJournalSelection();
            return true;
        }

        if (input == "exit" || input == "quit" || input == "leave")
        {
            ShowEnding();
            return true;
        }

        return false;
    }

    void ShowJournalSelection()
    {
        currentState = State.journal;
        Terminal.ClearScreen();
        Terminal.WriteLine("Select the entry you want to revisit:");
        foreach (string entry in journalEntries)
        {
            string firstLine = GetFirstLine(entry);
            Terminal.WriteLine("    " + firstLine);
        }

        Terminal.WriteLine("");
    }

    string GetFirstLine(string text)
    {
        return text.Split('\n')[0];
    }

    void ShowLevelSelectionError()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Invalid option selected.");
        Terminal.WriteLine("");
        Terminal.WriteLine("Choose one from the following:");
        ShowOptions();
    }

    void ShowPuzzle()
    {
        currentState = State.hacking;
        Terminal.ClearScreen();
        Terminal.WriteLine("Trying to hack " + hackables[level].GetName());
        Terminal.WriteLine("Error: password could not be fully hacked.");
        password = GetPassword();

        ShowPasswordHint(password);
    }

    void ShowPasswordHint(string password)
    {
        Terminal.WriteLine("Password retrieved but the letters are scrambled:");
        Terminal.WriteLine(password.Anagram());
        Terminal.WriteLine("");
        Terminal.WriteLine("Please input the correct password:");
    }

    string GetPassword()
    {
        return hackables[level].GetPasword();
    }

    void ProcessPasswordAttempt(string input)
    {
        if (input == password)
        {
            ShowVictory();
        } else
        {
            ShowRetry();
        }
    }

    void ShowRetry()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Password is not correct");
        Terminal.WriteLine("You can always type menu to go back");
        Terminal.WriteLine("");
        ShowPasswordHint(password);
        
    }

    void ShowVictory()
    {
        string winMessage = hackables[level].GetWinMessage();

        levelsSolved[level] += 1;
        if( level == 0)
        {
            journalEntries.Add(winMessage);
        }

        Terminal.ClearScreen();
        Terminal.WriteLine(winMessage);
        Terminal.WriteLine("");
        if(level == 0)
        {
            Terminal.WriteLine("Type journal to access all entries.");
        }
        Terminal.WriteLine("Type menu to continue.");
        currentState = State.hackResult;
    }

    void ShowEnding()
    {
        currentState = State.ending;

        Terminal.ClearScreen();
        ShowUnknownEnding();
        Terminal.WriteLine("");
        Terminal.WriteLine("Press enter to quit the game");
    }

    void ShowUnknownEnding()
    {
        Terminal.WriteLine(@"You got bored and went playing outside.

Your sibling didn't come back home that night. Or any night afterwards.

You never really knew what happened.
Maybe if you had listened to her more while she was around you'd know...");

    }
}