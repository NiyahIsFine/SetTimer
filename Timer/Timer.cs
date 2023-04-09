using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFramework;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public static Timer Instance;
    readonly List<TimerAction> updateActions = new();
    readonly List<TimerAction> fixedUpdateActions = new();
    readonly List<TimerAction> lateUpdateActions = new();
    readonly List<int> updateFreeAction = new();
    readonly List<int> fixedUpdateFreeAction = new();
    readonly List<int> lateUpdateFreeAction = new();
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static TimerAction Set(float time, Action action, UpdateMode updateMode = UpdateMode.Update)
    {
        if (!Instance)
        {
            throw new("TimerAcion.instance == null");
        }
        TimerAction timerAction = null;
        switch (updateMode)
        {
            case UpdateMode.Update:
                if (Instance.updateFreeAction.Count == 0)
                {
                    timerAction = new(time, action, updateMode);
                    Instance.updateActions.Add(timerAction);
                }
                else
                {
                    timerAction = Instance.updateActions[Instance.updateFreeAction[0]].Reset(time, action, updateMode);
                    Instance.updateFreeAction.RemoveAt(0);
                }
                break;
            case UpdateMode.FixedUpdate:
                if (Instance.fixedUpdateFreeAction.Count == 0)
                {
                    timerAction = new(time, action, updateMode);
                    Instance.fixedUpdateActions.Add(timerAction);
                }
                else
                {
                    timerAction = Instance.fixedUpdateActions[Instance.fixedUpdateFreeAction[0]].Reset(time, action, updateMode);
                    Instance.fixedUpdateFreeAction.RemoveAt(0);
                }
                break;
            case UpdateMode.LateUpdate:
                if (Instance.lateUpdateFreeAction.Count == 0)
                {
                    timerAction = new(time, action, updateMode);
                    Instance.lateUpdateActions.Add(timerAction);
                }
                else
                {
                    timerAction = Instance.lateUpdateActions[Instance.lateUpdateFreeAction[0]].Reset(time, action, updateMode);
                    Instance.lateUpdateFreeAction.RemoveAt(0);
                }
                break;
        }
        return timerAction;
    }
    private void Update()
    {
        for (int i = 0; i < updateActions.Count; i++)
        {
            if (updateActions[i].IsRunning)
            {
                if (updateActions[i].Update(updateActions[i].UnscaleTimeMode ? Time.unscaledDeltaTime : Time.deltaTime))
                {
                    updateActions[i].Free();
                    updateFreeAction.Add(i);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < fixedUpdateActions.Count; i++)
        {
            if (fixedUpdateActions[i].IsRunning)
            {
                if (fixedUpdateActions[i].Update(Time.fixedDeltaTime))
                {
                    fixedUpdateActions[i].Free();
                    fixedUpdateFreeAction.Add(i);
                }
            }
        }
    }
    private void LateUpdate()
    {
        for (int i = 0; i < lateUpdateActions.Count; i++)
        {
            if (lateUpdateActions[i].IsRunning)
            {
                if (lateUpdateActions[i].Update(lateUpdateActions[i].UnscaleTimeMode ? Time.unscaledDeltaTime : Time.deltaTime))
                {
                    lateUpdateActions[i].Free();
                    lateUpdateFreeAction.Add(i);
                }
            }
        }
    }
}
