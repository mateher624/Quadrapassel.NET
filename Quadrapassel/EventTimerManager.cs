using System;
using System.Collections.Generic;
using System.Timers;

namespace Quadrapassel
{
    public static class EventTimerManager
    {
        private static readonly ICollection<EventTimer> EventTimers = new List<EventTimer>();

        public static EventTimer Add(int interval, Func<bool> action)
        {
            var eventTimer = new EventTimer(interval, action);
            EventTimers.Add(eventTimer);
            return eventTimer;
        }

        public static void Remove(EventTimer eventTimer)
        {
            EventTimers.Remove(eventTimer);
            eventTimer.Dispose();
        }
    }

    public class EventTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly Func<bool> _action;

        public EventTimer(int interval, Func<bool> action)
        {
            _action = action;
            _timer = new Timer(interval);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var result = _action.Invoke();
            if (!result)
                Dispose();
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
