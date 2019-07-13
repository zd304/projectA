using System;
using UnityEngine;
using System.Collections.Generic;

public class CharacterManager
{
    private CharacterManager()
    {

    }

    static CharacterManager instance = null;
    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CharacterManager();
            }
            return instance;
        }
    }

    public void AddCharacter(Character cha)
    {
        if (cha == null)
        {
            return;
        }
        if (characters.ContainsKey(cha.id))
        {
            Debug.LogError("角色ID重复：" + cha.id + "，角色名：" + cha.name);
            return;
        }

        if (cha.characterType == CharacterType.Player)
        {
            if (MainPlayer)
            {
                return;
            }
            MainPlayer = cha;
        }

        characters.Add(cha.id, cha);
    }

    public Character GetCharacter(int id)
    {
        Character cha = null;
        if (characters.TryGetValue(id, out cha))
        {
            return cha;
        }
        return null;
    }

    public void RemoveCharacter(int id)
    {
        Character cha = null;
        if (!characters.TryGetValue(id, out cha))
        {
            return;
        }
        if (MainPlayer == cha)
        {
            MainPlayer = null;
        }
        characters.Remove(id);
    }

    public Character MainPlayer
    {
        private set;
        get;
    }

    private Dictionary<int, Character> characters = new Dictionary<int, Character>();
}