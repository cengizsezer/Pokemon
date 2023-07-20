using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EventManager
{
   public class EventHandler<T> where T : CustomEvent
    {
        public  List<Action<T>> lsEvents = new();
        private static EventHandler<T> Instance = null;
        private static EventHandler<T> instance { get => Instance ?? (Instance = new EventHandler<T>()); }

        public static void Register(Action<T> @Event)
        {
            if (!instance.lsEvents.Contains(@Event))
            {
                instance.lsEvents.Add(@Event);
            }
        }

        public static void UnRegister(Action<T> @Event)
        {
            if (instance.lsEvents.Contains(@Event))
            {
                instance.lsEvents.Remove(@Event);
            }
        }

        public static void Handle(T Parametre)
        {
            var EndElement = instance.lsEvents.Count - 1;

            for (int i = EndElement; i >= 0; i--)
            {
                Instance.lsEvents[i](Parametre);
            }
        }

        public static void DelayHandle(T Parametre,float DelayValue,MonoBehaviour parent)
        {
            var EndElement = instance.lsEvents.Count - 1;
            for (int i = EndElement; i >= 0; i--)
            {
                new DelayedAction(()=> instance.lsEvents[i](Parametre),DelayValue).Execute(parent);
            }
        }
    }


    public static void Add<T>(Action<T> Action) where T:CustomEvent
    {
        EventHandler<T>.Register(Action);
    }

    public static void Remove<T>(Action<T> Action) where T:CustomEvent
    {
        EventHandler<T>.UnRegister(Action);
    }


    public static void Send<T>(T Parametre) where T : CustomEvent 
    {
        EventHandler<T>.Handle(Parametre);
    }

    public static void DelayedSend<T>(T Parametre,float DelayValue,MonoBehaviour Parent) where T : CustomEvent
    {
        EventHandler<T>.DelayHandle(Parametre, DelayValue, Parent);
    }
}
