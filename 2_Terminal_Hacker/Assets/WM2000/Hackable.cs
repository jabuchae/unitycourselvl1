using UnityEngine;
using System.Collections.Generic;
using System.Linq;

interface Hackable
{
    string GetName();
    string GetPasword();
    string GetWinMessage();
}

class SiblingJournal : Hackable
{
    private const string name = "a random entry on your sibling's online journal";
    private Dictionary<string, string> passwords = new Dictionary<string, string>();
    private string[] passwordKeys;
    private readonly string[] winMessages = new string[]
    {
    };

    public SiblingJournal()
    {
        passwords.Add("Jenny",
            @"Feb 2nd, 2003
    I went over to our neighbours for Margareth's birthday today.
    Can't believe she's 12 already!

    Jenny came along too.
    We had a lot of fun and ate a lot of cake."
    );

        passwords.Add("David",
            @"Jul 17th, 2005
    I don't understand David.
    He's been avoiding me since that stupid argument with the Jeffersons.

    I don't care what he thinks.
    They are nice people and they love Margareth."
    );
        passwords.Add("Mandy",
            @"Feb 2nd, 2006
    I took Mandy over to the Jeffersons for Margareth's birthday.

    It was nice. I like being around Mandy.
    She makes me happy."
    );
        passwords.Add("Johnny",
            @"Jan 20th, 2007
    I'm bored.

    I though I'd write a bit while waiting for Johnny.

    He's ALWAYS late..."
    );
        passwords.Add("Lenny",
            @"Feb 2nd, 2007
    I'm bored. Just waiting for Lenny to go to Maggie's birthday.

    I just hope she's ok, I heard some screaming coming from her house...
    Screw Lenny, I'm going over now to check on Maggie!"
    );


        passwordKeys = RandomizePasswords();
    }

    private string[] RandomizePasswords()
    {
        string[] keys = new string[passwords.Keys.Count];
        passwords.Keys.CopyTo(keys, 0);

        return keys;
    }
    public string GetName()
    {
        return name;
    }

    public string GetPasword()
    {
        return passwordKeys[0];
    }

    public string GetWinMessage()
    {
        var currentPassword = passwordKeys[0];

        passwordKeys = passwordKeys.Skip(1).ToArray(); // Remove the current key

        return passwords[currentPassword];
    }
}

class NeighbourWifi : Hackable
{
    private const string name = "your neighbour's WiFi";
    private const string password = "Margareth1993";
    private bool alreadyWon = false;

    public NeighbourWifi()
    {
    }

    public string GetName()
    {
        return name;
    }

    public string GetPasword()
    {
        return password;
    }

    public string GetWinMessage()
    {
        alreadyWon = true;
        return "You've hacked into your neighbourh's WiFi and you are now in their local network.";
    }
}