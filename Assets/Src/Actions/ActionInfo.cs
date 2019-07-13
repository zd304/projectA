using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ActionInfo : Attribute
{
    public ActionInfo(string actionName, string menuDirectory, string description)
    {
        this.actionName = actionName;
        this.menuDirectory = menuDirectory;
        this.description = description;
    }

    public string actionName = string.Empty;
    public string menuDirectory = string.Empty;
    public string description = string.Empty;
}