using System.Collections.Generic;
using Assets.Scripts.Client.Events;

namespace Assets.Scripts.Common
{
    public class ClientEventAggregator
    {
        public static Queue<IActionEvent> EventQueue { get; } = new Queue<IActionEvent>();

        private static readonly List<IEventHandler> subscribers = new List<IEventHandler>();

        public static void Publish(IActionEvent actionEventSend)
        {
            EventQueue.Enqueue(actionEventSend);
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].HandleEvent(actionEventSend);
            }
        }

        public static void Subscribe(IEventHandler subscriber)
        {
            subscribers.Add(subscriber);
        }

        public static void Unsubscribe(IEventHandler subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public static void ResetQueue()
        {
            EventQueue.Clear();
        }
    }
}
