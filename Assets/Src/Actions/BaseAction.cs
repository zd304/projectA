using System;
using UnityEngine;

public class BaseAction : MonoBehaviour
{
    public int actionID = 0;
    public int nextActionID = 0;

    public ActionFlow flow = null;

    public ActionContext context
    {
        get
        {
            if (flow)
            {
                return flow.context;
            }
            return null;
        }
    }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void OnUpdate() { }

    public virtual void Continue()
    {
        OnExit();

        BaseAction next = null;
        if (!flow.id2Actions.TryGetValue(nextActionID, out next))
        {
            return;
        }

        flow.CurrentAction = next;

        next.OnEnter();
    }
}