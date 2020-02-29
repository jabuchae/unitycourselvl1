using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    public string username = "";

    private Hackable[] hackables = new Hackable[] {
        new Hackable("your sibling's Facebook", new string[] { "Jenny", "David", "Mandy", "Johnny", "Lenny"}),
        new Hackable("your neighbour's WiFi", new string[] { "Margareth1993", "Metallica81", "Edinburgh2014", "Try2GuessMe", "OurWifiPassword"}),
    };

    private string state = "menu";
    private string password;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu(username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowTrophy()
    {
        Terminal.WriteLine("  _______");
        Terminal.WriteLine(" |       |");
        Terminal.WriteLine("(|       |)");
        Terminal.WriteLine(" |       |");
        Terminal.WriteLine("  \\     /");
        Terminal.WriteLine("   `---'");
        Terminal.WriteLine(" _ | _ | _");
    }

    void ShowMainMenu(string username)
    {
        state = "menu";
        Terminal.ClearScreen();
        Terminal.WriteLine("Hello " + username + "!");
        Terminal.WriteLine("Boring day isn't it?");
        Terminal.WriteLine("Why not hack someone?");
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
        switch (state)
        {
            case "menu":
                ProcessMenuInput(input);
                break;
            case "hacking":
                ProcessPasswordAttempt(input);
                break;
            case "end":
                ShowMainMenu(username);
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
        state = "hacking";
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
        var passwords = hackables[level].GetPaswords();
        var randomIndex = (int) Random.Range(0, passwords.Length);
        return passwords[randomIndex];
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
        Terminal.WriteLine("Congratulations!");
        Terminal.WriteLine("You hacked into " + hackables[level].GetName());
        ShowTrophy();
        Terminal.WriteLine("");
        Terminal.WriteLine("Press any key to start over");
        state = "end";
    }
}

class Hackable
{
    private string name;
    private string[] passwords;

    public Hackable(string name, string[] passwords)
    {
        this.name = name;
        this.passwords = passwords;
    }

    public string GetName()
    {
        return name;
    }

    public string[] GetPaswords()
    {
        return passwords;
    }
}