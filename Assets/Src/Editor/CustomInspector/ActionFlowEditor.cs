using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(ActionFlow))]
public class ActionFlowEditor : Editor
{
    ActionFlow Target
    {
        get
        {
            return target as ActionFlow;
        }
    }

    void Callback(object obj)
    {
        Debug.Log("Selected: " + obj.ToString());

        Type type = obj as Type;
        Target.CreateAction(type);
    }

    List<Type> GetDeriveTypes(Type baseType)
    {
        List<Type> rst = new List<Type>();

        var types = Assembly.GetAssembly(baseType).GetTypes();
        foreach (var t in types)
        {
            var tmp = t.BaseType;
            while (tmp != null)
            {
                if (tmp == baseType)
                {
                    rst.Add(t);
                    break;
                }
                tmp = tmp.BaseType;
            }
        }
        return rst;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button(new GUIContent("创建新行为")))
        {
            GameObject targetGO = Target.gameObject;
            BaseAction[] actions = targetGO.GetComponents<BaseAction>();

            Dictionary<string, Type> menuToType = new Dictionary<string, Type>();
            GenericMenu menu = new GenericMenu();

            Type type = typeof(BaseAction);
            List<Type> actionTypes = GetDeriveTypes(type);

            for (int i = 0; i < actionTypes.Count; ++i)
            {
                type = actionTypes[i];

                object[] aiobjs = type.GetCustomAttributes(typeof(ActionInfo), false);
                ActionInfo[] actionInfos = aiobjs as ActionInfo[];

                if (actionInfos.Length > 0)
                {
                    ActionInfo actionInfo = actionInfos[0];
                    string menuContent = actionInfo.menuDirectory + "/" + actionInfo.actionName;
                    menuToType.Add(menuContent, type);

                    menu.AddItem(new GUIContent(menuContent), false, Callback, type);
                }
            }

            menu.ShowAsContext();            
        }
    }
}