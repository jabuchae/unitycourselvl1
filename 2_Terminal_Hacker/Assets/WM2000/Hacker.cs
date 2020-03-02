using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    // Levels
    private Hackable[] hackables;

    // Different states of the game
    enum State { menu, hacking, ending }

    // Game State
    private State currentState = State.menu;
    private string password;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        hackables = new Hackable[] {
            new SiblingJournal(),
            new NeighbourWifi()
        };
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
        Terminal.WriteLine("\tBoring day isn't it?");
        Terminal.WriteLine("");

        ShowOptions();
    }

    void ShowOptions()
    {
        for (int i = 0; i < hackables.Length; i++)
        {
            Terminal.WriteLine("Press " + (i+1) + " for " + hackables[i].GetName());
        }
    }

    void OnUserInput(string input)
    {
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
            case State.ending:
                break;
            default:
                break;
        }
        
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

        return false;
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

        password = GetPassword();

        ShowPasswordHint(password);
    }

    void ShowPasswordHint(string password)
    {
        Terminal.WriteLine("Password letters are:");
        Terminal.WriteLine(password.Anagram());
        Terminal.WriteLine("");
        Terminal.WriteLine("Input password:");
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
        Terminal.WriteLine("");
        ShowPasswordHint(password);
    }

    void ShowVictory()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine(hackables[level].GetWinMessage());

        Terminal.WriteLine("");
        Terminal.WriteLine("Type menu to continue.");
        currentState = State.ending;
    }
}