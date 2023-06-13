using System.Collections.Generic;
using UnityEngine;
using static Types;

public enum GameState
{
    Start,
    play,
    end
}
// The Game Events used across the Game.
// Anytime there is a need for a new event, it should be added here.

public static class Events
{
    
    public static BlueToothReciveEvent BlueToothReciveEvent = new BlueToothReciveEvent();
    public static AllObjectivesCompletedEvent AllObjectivesCompletedEvent = new AllObjectivesCompletedEvent();
    public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
    public static ChangeCameraEvent ChangeCameraEvent = new ChangeCameraEvent();
    public static SceneEvent SceneEvent = new SceneEvent();

    public static GameStateChangeEvent GameStateChangeEvent = new GameStateChangeEvent();
    public static InputEvent InputEvent = new InputEvent();
    public static SeriportReciveEvent SeriportReciveEvent = new SeriportReciveEvent();
    public static RedArmyStateEvent redArmyStateEvent = new RedArmyStateEvent();
}


public class RedArmyStateEvent : GameEvent
{
    public RedArmyState redArmyState;
}

public class SceneEvent : GameEvent
{
    public int index;
}
public class AllObjectivesCompletedEvent : GameEvent { }
public class BlueToothReciveEvent : GameEvent
{
    public string data;
}
public class SeriportReciveEvent : GameEvent
{
    public byte[] data;
}
public class GameOverEvent : GameEvent
{
    public bool Win;
}
public class ChangeCameraEvent : GameEvent
{
    public int cameraIndex;
}
public class DisplayMessageEvent : GameEvent
{
    public string Message;
    public float DelayBeforeDisplay;
}


public class GameStateChangeEvent : GameEvent
{
    public GameState gameState;
}

public class InputEvent : GameEvent
{
    public string inputKey;
}