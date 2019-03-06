using System.Collections.Generic;
using UnityEngine;

enum FlagState
{
    None,
    Required,
    Obtained,
    Both
}

[System.Serializable]
class FlagAndState
{
    public string flag;
    public FlagState state;
}

[ExecuteInEditMode]
public class FlagChecker : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    List<FlagAndState> flags;

    int GetFlagIndex(string flag)
    {
        return flags.FindIndex(item => item.flag == flag);
    }

    private void Awake()
    {
        PlayerFlags.AssignFlagChecker(this);
    }

    public void RegisterFlagRequirement(string flag)
    {
        int index = GetFlagIndex(flag);
        if (index == -1)
        {
            flags.Add(new FlagAndState { flag = flag, state = FlagState.Required });
        }
        else if (flags[index].state == FlagState.Obtained)
            flags[index].state = FlagState.Both;
    }

    public void RegisterFlagObtainment(string flag)
    {
        int index = GetFlagIndex(flag);
        if (index == -1)
        {
            flags.Add(new FlagAndState { flag = flag, state = FlagState.Obtained });
        }
        else if (flags[index].state == FlagState.Required)
            flags[index].state = FlagState.Both;
    }
}
