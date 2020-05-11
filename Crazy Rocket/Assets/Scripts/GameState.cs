using System;
public class GameState
{
    static public GameState instance = new GameState();

    private Status currentStatus;

    private GameState()
    {
        currentStatus = Status.StartLevel;
    }

    public void SetStatus(Status newStatus)
    {
        currentStatus = newStatus;
    }

    public Status GetStatus()
    {
        return currentStatus;
    }

    public bool IsPlayerActive()
    {
        bool playerActive = false;

        switch (currentStatus)
        {
            case Status.Menu:
            case Status.Pause:
            case Status.StartLevel:
            case Status.WinLevel:
            case Status.Dying:
                playerActive = false;
                break;
            case Status.Playing:
                playerActive = true;
                break;
        }

        return playerActive;
    }

    public enum Status
    {
        Pause,
        Playing,
        WinLevel,
        StartLevel,
        Dying,
        Menu
    }
}
