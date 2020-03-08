using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    // Levels
    private List<Hackable> hackables;

    // Different states of the game
    enum State { menu, hacking, hackResult, journal, ending }

    // Game State
    private State currentState;
    private string password;
    private int level;

    private List<string> journalEntries;

    private int[] levelsSolved;
    private readonly int[] maxPuzzles = { 4, 1, 4 };

    // Faster menus
    private bool mainMenuShown = false;
    private bool hackAttemptShown = false;

    // Start is called before the first frame update
    void Start()
    {
        hackables = new List<Hackable>();
        hackables.Add(new SisterJournal());
        hackables.Add(new NeighbourWifi());

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
        PacedWriter.usePacing = !mainMenuShown;
        currentState = State.menu;
        Terminal.ClearScreen();
        PacedWriter.WriteLine("Feb 1st, 2007");
        PacedWriter.WriteLine("    Boring day isn't it?");
        PacedWriter.WriteLine("");
        ShowOptions();
        PacedWriter.usePacing = true;
        mainMenuShown = true;
    }

    void ShowOptions()
    {
        for (int i = 0; i < hackables.Count; i++)
        {
            if (levelsSolved[i] < maxPuzzles[i])
            {
                PacedWriter.WriteLine("Press " + (i + 1) + " to hack into " + hackables[i].GetName());
                PacedWriter.WriteLine("");
            }
        }

        ShowMenuHint();
    }

    void ShowMenuHint()
    {
        string journalAccess = levelsSolved[0] >= 1 ? ", journal" : "";
        PacedWriter.WriteLine("You can always type menu" + journalAccess + " or quit.");
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
        clearText = clearText.Replace("1st", "1");
        clearText = clearText.Replace("2nd", "2");
        clearText = clearText.Replace("20th", "20");
        clearText = clearText.Replace("17th", "17");
        
        return clearText;
    }

    void ShowJournalEntry(string entry)
    {
        PacedWriter.usePacing = false;
        Terminal.ClearScreen();
        PacedWriter.WriteLine(entry);
        PacedWriter.WriteLine("");
        PacedWriter.WriteLine("Type menu to continue or journal to see more entries.");
        PacedWriter.usePacing = true;
    }

    void ProcessMenuInput(string input)
    {
        int level = -1;
        for (int i = 0; i < hackables.Count; i++)
        {
            if (input == (i+1).ToString() && levelsSolved[i] < maxPuzzles[i])
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
        PacedWriter.WriteLine("Select the entry you want to revisit:");
        foreach (string entry in journalEntries)
        {
            string firstLine = GetFirstLine(entry);
            PacedWriter.WriteLine("    " + firstLine);
        }

        PacedWriter.WriteLine("");
    }

    string GetFirstLine(string text)
    {
        return text.Split('\n')[0];
    }

    void ShowLevelSelectionError()
    {
        Terminal.ClearScreen();
        PacedWriter.WriteLine("Invalid option selected.");
        PacedWriter.WriteLine("");
        PacedWriter.WriteLine("Choose one from the following:");
        ShowOptions();
    }

    void ShowPuzzle()
    {

        PacedWriter.usePacing = !hackAttemptShown;

        currentState = State.hacking;
        Terminal.ClearScreen();
        PacedWriter.WriteLine("Trying to hack " + hackables[level].GetName());
        PacedWriter.WriteLine("Error: password could not be fully hacked.");
        password = GetPassword();

        ShowPasswordHint(password);

        PacedWriter.usePacing = true;
        hackAttemptShown = true;
    }

    void ShowPasswordHint(string password)
    {
        PacedWriter.WriteLine("Password retrieved but the letters are scrambled:");
        PacedWriter.WriteLine(password.Anagram());
        PacedWriter.WriteLine("");
        PacedWriter.WriteLine("Please input the correct password:");
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
            if( ShouldEnableCameras())
            {
                EnableCameras();
            }
        } else
        {
            ShowRetry();
        }
    }

    void ShowRetry()
    {
        Terminal.ClearScreen();
        PacedWriter.WriteLine("Password is not correct");
        PacedWriter.WriteLine("You can always type menu to go back");
        PacedWriter.WriteLine("");
        ShowPasswordHint(password);
        
    }

    void ShowVictory()
    {
        currentState = State.hackResult;

        string winMessage = hackables[level].GetWinMessage();
        hackables[level].solved(this);

        levelsSolved[level] += 1;
        if( level == 0)
        {
            journalEntries.Add(winMessage);
        }

        Terminal.ClearScreen();
        PacedWriter.WriteLine(winMessage);
        PacedWriter.WriteLine("");
        if(level == 0)
        {
            PacedWriter.WriteLine("Type journal to access all entries.");
        }
        PacedWriter.WriteLine("Type menu to continue.");
    }

    void ShowEnding()
    {
        currentState = State.ending;

        Terminal.ClearScreen();
        ShowUnknownEnding();
        PacedWriter.WriteLine("");
        PacedWriter.WriteLine("Press enter to quit the game");
    }

    void ShowUnknownEnding()
    {
        PacedWriter.WriteLine(@"You got bored and went playing outside.

Your sister didn't come back home that night. Or any night afterwards.

You never really knew what happened.
Maybe if you had listened to her more while she was around you'd know...");

    }


    public bool ShouldEnableCameras()
    {
        return levelsSolved[0] == maxPuzzles[0] && levelsSolved[1] == maxPuzzles[1];
    }

    public void EnableCameras()
    {
        hackables.Add(new NeighbourSecurityCamera());
    }

    public void addJournalEntry()
    {
        maxPuzzles[0] += 1;
    }
}