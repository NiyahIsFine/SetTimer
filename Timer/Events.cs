using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework
{
    public class Events
    {
        public float lastTime;
        public List<Event> actions;
        public Action onEnd;
        public Events(float lastTime)
        {
            this.lastTime = lastTime;
            actions = new();
        }
        public void Add(float time, Action action)
        {
            if (actions.Count < 1)
            {
                actions.Add(new(time, action));
                return;
            }
            else
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i].time > time)
                    {
                        actions.Insert(i, new(time, action));
                        return;
                    }
                }
            actions.Add(new(time, action));
        }
        public void OnEnd(Action action)
        {
            onEnd += action;
        }
        public void Remove(float time)
        {
            List<Event> _actions = new();
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].time != time)
                {
                    _actions.Add(actions[i]);
                }
            }
            actions = _actions;
        }
        public void Clear()
        {
            actions.Clear();
            onEnd = null;
        }
    }

    public struct Event
    {
        public float time;
        public Action action;
        public bool executed;
        public Event(float time, Action action)
        {
            this.time = time;
            this.action = action;
            executed = false;
        }
        public Event Invoke()
        {
            action?.Invoke();
            executed = true;
            return this;
        }
    }
}