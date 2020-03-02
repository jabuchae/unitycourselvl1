using UnityEngine;
using System.Collections.Generic;
using System.Linq;

interface Hackable
{
    string GetName();
    string GetPasword();
    string GetWinMessage();
    bool isEnabled();
}

class SiblingJournal : Hackable
{
    private const string name = "Your sibling's online journal";
    private Dictionary<string, string> passwords = new Dictionary<string, string>();
    private string[] passwordKeys;
    private readonly string[] winMessages = new string[]
    {
    };

    public SiblingJournal()
    {
        passwords.Add("Jenny",
            @"
Feb 2nd, 2003
    I went over to our neighbours for Margareth's birthday today.
    Can't believe she's 12 already!

    Jenny came too so I wouldn't go alone.
    We had a lot of fun and ate a lot of cake."
    );

        passwords.Add("David",
            @"
Jul 17th, 2005
    I don't understand what's going on with David.
    He's been avoiding me since that stupid argument with the Jeffersons.

    I don't care what he thinks.
    They are nice people and they love their daughter."
    );
        passwords.Add("Mandy",
            @"
Feb 2nd, 2006
    I took Mandy over to the Jeffersons for Maggie's bday.

    It was nice. I like being around Mandy.
    She makes me happy."
    );
        passwords.Add("Johnny",
            @"
Jan 20th, 2007
    I'm bored.

    I though I'd write a bit while waiting for Johnny.

    He's ALWAYS late..."
    );
        passwords.Add("Lenny",
            @"
Feb 2nd, 2007
    I woke up and I felt like writing.

    Have to wait for Lenny to go to Maggie's birthday anyway.

    I just hope she's ok, I heard some screaming coming from her house...

    Screw Lenny, I'm going over now to check on Maggie!
"
    );


        passwordKeys = RandomizePasswords();
    }

    private string[] RandomizePasswords()
    {
        string[] keys = new string[5];
        passwords.Keys.CopyTo(keys, 0);

        for (int i = 0; i < 10; i++)
        {
            var key1 = Random.Range(0, keys.Length);
            var key2 = Random.Range(0, keys.Length);

            var aux = keys[key1];
            keys[key1] = keys[key2];
            keys[key2] = aux;
        }
        

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

    public bool isEnabled()
    {
        return passwordKeys.Length != 0;
    }
}

class NeighbourWifi : Hackable
{
    private const string name = "Your neighbour's WiFi";
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

    public bool isEnabled()
    {
        return !alreadyWon;
    }
}