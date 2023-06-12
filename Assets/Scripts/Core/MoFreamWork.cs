using System.Collections;
using System.Collections.Generic;
using System;
public class MoFreamWork
{

}
public interface IMActer
{
    void Init();
}
public class BundToValue<T>
{
    public BundToValue(T defultValue)
    {
        mValue = defultValue;
    }
    protected T mValue;
    public T Value
    {
        get => GetValue();
        set
        {
            if (value == null && mValue == null) return;
            if (value != null && value.Equals(mValue)) return;

            SetValue(value);
            mOnValueChanged?.Invoke(value);
        }
    }

    protected virtual void SetValue(T newValue)
    {
        mValue = newValue;
    }

    protected virtual T GetValue()
    {
        return mValue;
    }

    public void SetValueWithoutEvent(T newValue)
    {
        mValue = newValue;
    }

    private Action<T> mOnValueChanged = (v) => { };

    public void Register(Action<T> onValueChanged)
    {
        mOnValueChanged += onValueChanged;
    }

    public void RegisterWithInitValue(Action<T> onValueChanged)
    {
        onValueChanged(mValue);
    }
    public override string ToString()
    {
        return Value.ToString();
    }
    public void UnRegister(Action<T> onValueChanged)
    {
        mOnValueChanged -= onValueChanged;
    }
}

public class Singleton<T> where T : IMActer,new()
{
    private static T ms_instance;

    public static T Instance
    {
        get
        {
            if (ms_instance == null)
            {
                ms_instance = new T();
                ms_instance.Init();
            }
            return ms_instance;
        }
    }
}

public class GameEvent{}

public static class EventManager
{
    static readonly Dictionary<Type, Action<GameEvent>> s_Events = new Dictionary<Type, Action<GameEvent>>();

    static readonly Dictionary<Delegate, Action<GameEvent>> s_EventLookups =
        new Dictionary<Delegate, Action<GameEvent>>();

    public static void AddListener<T>(Action<T> evt) where T : GameEvent
    {
        if (!s_EventLookups.ContainsKey(evt))
        {
            Action<GameEvent> newAction = (e) => evt((T)e);
            s_EventLookups[evt] = newAction;

            if (s_Events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
                s_Events[typeof(T)] = internalAction += newAction;
            else
                s_Events[typeof(T)] = newAction;
        }
    }

    public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
    {
        if (s_EventLookups.TryGetValue(evt, out var action))
        {
            if (s_Events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    s_Events.Remove(typeof(T));
                else
                    s_Events[typeof(T)] = tempAction;
            }

            s_EventLookups.Remove(evt);
        }
    }

    public static void Broadcast(GameEvent evt)
    {
        if (s_Events.TryGetValue(evt.GetType(), out var action))
            action.Invoke(evt);
    }

    public static void Clear()
    {
        s_Events.Clear();
        s_EventLookups.Clear();
    }
}