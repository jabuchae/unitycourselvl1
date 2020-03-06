using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public interface Hackable
{
    string GetName();
    string GetPasword();
    string GetWinMessage();
    void solved(Hacker hacker);
}

class SisterJournal : Hackable
{
    private const string name = "a random entry on your sister's online journal";
    private Dictionary<string, string> passwords = new Dictionary<string, string>();
    private string[] passwordKeys;

    public SisterJournal()
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

        passwords.Add("Maggie",
            @"Feb 2nd, 2007
    Maggie told her parents about us today... it didn't go well.

Her father almost destroyed the livingroom with his outburst. Luckily for us, Maggie's mother couldn't take it and went crying to her room. Her father went to check on her and took that change to barricade them in and escape.

I wish I could have said goodby to my family, but I fear they'll react like the Jeffersons did.

Maybe when things settle. Maybe..."
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
        return passwords[passwordKeys[0]];
    }

    public void solved(Hacker hacker)
    {
        passwordKeys = passwordKeys.Skip(1).ToArray(); // Remove the current key
    }
}

class NeighbourWifi : Hackable
{
    private const string name = "your neighbour's WiFi";
    private const string password = "Margareth1993";

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
        return "You've hacked into your neighbourh's WiFi and you are now in their local network.";
    }

    public void solved(Hacker hacker)
    {
    }
}

class NeighbourSecurityCamera : Hackable
{
    private const string name = "your neighbour's security cameras";
    private Dictionary<string, string> passwords = new Dictionary<string, string>();
    private string[] passwordKeys;

    public NeighbourSecurityCamera()
    {
        passwords.Add("Kitchen",
               @"You hack into the kitchen security camera.

There's a pile of dirty dishes in the sink and a chocolate cake right in the middle of the kitchen table.

As the camrea turns left, you manage to catch a glimps of someone going upstairs."
       );

        passwords.Add("Bedroom",
            @"You hack into the bedroom security camera.

You see Ms. Jefferson sitting on the edge of the bed, wiping while holding the very end of her necklace in both hands.

Mr. Jefferson is there too. Charging toward the bedroom door and hitting it in every way possible, trying to burst it open.

They are trapped."
    );
        passwords.Add("Livingroom",
            @"The livingroom looks quite bad.

You notice the front door is open.

There's glass in the floor and a lamp is smashed against the couch."
    );
        passwords.Add("Porch",
            @"You see a small portion of the porch, from the top of the door.

There's your sister.

You see her only for an instant as someone is grabbing her by the hand as she is dragged outside the house."
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
        return passwords[GetPasword()];
    }

    public void solved(Hacker hacker)
    {
        passwordKeys = passwordKeys.Skip(1).ToArray(); // Remove the current key

        if (passwordKeys.Length == 0)
        {
            hacker.addJournalEntry();
        }
    }
}
