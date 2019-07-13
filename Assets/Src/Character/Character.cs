using UnityEngine;

public enum CharacterType
{
    Default,
    Player,
    Enemy,
}

public class Character : MonoBehaviour
{
    private void Awake()
    {
        CharacterManager.Instance.AddCharacter(this);
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.RemoveCharacter(id);
    }

    private Character target;
    public Character Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
    }

    public int id = 0;
    public CharacterType characterType = CharacterType.Default;
}