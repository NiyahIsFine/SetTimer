using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework
{
    public class TimerAction
    {
        public UpdateMode updateMode;
        public float lastTime;
        internal Action action;
        public Events Events;
        internal Action delegateDestroy;
        public Action DelegateDestroy => delegateDestroy;
        public void DelegateDestroyFun()
        {
            DelegateDestroy?.Invoke();
        }
        internal float time;
        public float Time => time;
        public float NormalizedTime => Time / lastTime;

        bool isRunning;
        public bool IsRunning => isRunning && !free;

        bool start;

        internal bool free;
        bool unscaleTimeMode;
        public bool UnscaleTimeMode => unscaleTimeMode;
        public TimerAction(float lastTime, Action action, UpdateMode updateMode)
        {
            this.lastTime = lastTime;
            this.action = action;
            this.updateMode = updateMode;
            Events = new(lastTime);
            time = 0;
            isRunning = true;
            free = false;
            unscaleTimeMode = false;
            delegateDestroy += Free;
        }
        internal TimerAction Reset(float lastTime, Action action, UpdateMode updateMode)
        {
            this.lastTime = lastTime;
            this.action = action;
            this.updateMode = updateMode;
            time = 0;
            isRunning = true;
            free = false;
            unscaleTimeMode = false;
            return this;
        }
        public void Free()
        {
            free = true;
            Events.Clear();

        }
        public TimerAction Stop()
        {
            isRunning = false;
            return this;
        }
        public TimerAction Start()
        {
            isRunning = true;
            return this;
        }

        public TimerAction SetUnscaleTime(bool active)
        {
            unscaleTimeMode = active;
            return this;
        }

        //执行完后return true
        internal bool Update(float dt)
        {
            if (!start)
            {
                start = true;
            }
            else
            {
                time += dt;
            }

            for (int i = 0; i < Events.actions.Count; i++)
            {
                if (Events.actions[i].executed)
                    continue;
                if (Events.actions[i].time <= Time)
                {
                    try
                    {
                        Event _event = Events.actions[i];
                        Events.actions[i] = _event.Invoke();
                    }
                    catch
                    {
                        throw new($"Events.actions[{i}] exception");
                    }
                }
            }

            if (Time >= lastTime)
            {
                try
                {
                    action?.Invoke();
                }
                catch
                {
                    throw new($"actions exception");
                }
                try
                {
                    Events.onEnd?.Invoke();
                }
                catch
                {
                    throw new($"   Events.onEnd exception");
                }
                return true;
            }

            return false;
        }
    }
    public enum UpdateMode
    {
        Update,
        FixedUpdate,
        LateUpdate,
    }

}