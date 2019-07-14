using System;
using UnityEngine;
using System.Collections.Generic;

public class ActionFlow : MonoBehaviour
{
    private void Awake()
    {
        id2Actions.Clear();
        for (int i = 0; i < actions.Count; ++i)
        {
            BaseAction action = actions[i];
            id2Actions.Add(action.actionID, action);
        }

        context.owner = GetComponent<Character>();
    }

    private void Start()
    {
        BaseAction action = null;
        if (id2Actions.TryGetValue(startActionID, out action))
        {
            CurrentAction = action;
            CurrentAction.OnEnter();
        }
    }

    int GetEmptyActionID()
    {
        int id = 1;
        while (true)
        {
            if (!id2Actions.ContainsKey(id))
            {
                return id;
            }
            ++id;
        }
    }

    public BaseAction CreateAction(Type type)
    {
        int id = GetEmptyActionID();

        BaseAction t = gameObject.AddComponent(type) as BaseAction;
        t.actionID = id;
        t.flow = this;

        id2Actions.Add(id, t);
        actions.Add(t);
        return t;
    }

    public void Update()
    {
        if (CurrentAction)
        {
            CurrentAction.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (CurrentAction)
        {
            CurrentAction.OnFixedUpdate();
        }
    }

    public BaseAction CurrentAction
    {
        set;
        get;
    }

    [NonSerialized]
    public Dictionary<int, BaseAction> id2Actions = new Dictionary<int, BaseAction>();

    public int startActionID = 0;
    public ActionContext context = new ActionContext();
    public List<BaseAction> actions = new List<BaseAction>();
}