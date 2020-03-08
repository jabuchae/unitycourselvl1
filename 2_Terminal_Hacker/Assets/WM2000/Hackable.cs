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
        passwords.Add("Jimmy",
            @"Feb 2nd, 2005
    I went over to our neighbours for Margareth's sweet sixteen.

    Jimmy came along too.
    We had a so much fun and we ate A LOT."
    );

        passwords.Add("David",
            @"Jul 17th, 2005
    I don't understand David...
    He's so wrong about the Jeffersons.

    I don't care what he thinks.
    They are nice people and they love Margareth."
    );
        passwords.Add("Mandy",
            @"Feb 2nd, 2006
    I took Mandy over to the Jeffersons for Maggie's birthday today.

    It was... ok I guess?

    Maggie was a bit distant.
    I think she didn't like Mandy."
    );
        passwords.Add("Birthday",
            @"Feb 1st, 2007
    Got a present for Maggie at the mall.
    I think she'll like it!

    I just hope she's ok, I heard some screaming coming from her house...

    I'd better go check on her."
    );

        passwords.Add("Maggie",
            @"Feb 1st, 2007
    Maggie told her parents about us today...
It didn't go well.

Her father almost destroyed the livingroom with his outburst.
Luckily for us, Maggie's mother couldn't take it and went crying to her room.
Her father went to check on her and took that change to barricade them in and escape.

I wish I could have said goodby to my family, but I fear they'll react like the Jeffersons did.

Maybe when things settle.

Maybe..."
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
               @"There's a pile of dirty dishes in the sink and a chocolate cake right in the middle of the kitchen table.

You manage to catch a glimpse of someone going upstairs."
       );

        passwords.Add("Bedroom",
            @"You see Ms. Jefferson sitting on the edge of the bed.
Weeping while holding the very end of her necklace in both hands.

Mr. Jefferson is there too.
Charging toward the bedroom door and hitting it in every way possible, trying to burst it open."
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
