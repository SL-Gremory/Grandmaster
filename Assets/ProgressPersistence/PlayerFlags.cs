using System.Collections.Generic;

public static class PlayerFlags
{
    static HashSet<string> flags = new HashSet<string>();
    static FlagChecker checker;


    public static void LoadFlags(string[] ownedFlags)
    {
        flags = new HashSet<string>(ownedFlags);
    }

    public static void AddFlag(string flag)
    {
        if (flag.Length == 0)
            return;
        var sep = flag.Split(';');
        for (int i = 0; i < sep.Length; i++)
        {
            flags.Add(sep[i]);
        }
    }

    public static void RemoveFlag(string flag)
    {
        if (flag.Length == 0)
            return;
        var sep = flag.Split(';');
        for (int i = 0; i < sep.Length; i++)
        {
            flags.Remove(flag);
        }
    }

    public static bool HasFlag(string flag)
    {
        if (flag.Length == 0)
            return true;
        var sep = flag.Split(';');
        for (int i = 0; i < sep.Length; i++)
        {
            if (!flags.Contains(sep[i]))
                return false;
        }
        return true;
    }

    public static string[] GetFlags()
    {
        string[] all = new string[flags.Count];
        int i = 0;
        foreach (var flag in flags)
        {
            all[i] = flag;
            ++i;
        }
        return all;
    }

    public static void AssignFlagChecker(FlagChecker checker)
    {
        PlayerFlags.checker = checker;
    }

    public static void RegisterFlagRequirement(string flag)
    {
        if (checker != null)
        {
            if (flag.Length == 0)
                return;
            var sep = flag.Split(';');
            for (int i = 0; i < sep.Length; i++)
            {
                checker.RegisterFlagRequirement(sep[i]);
            }
        }

    }

    public static void RegisterFlagObtainment(string flag)
    {
        if (checker != null)
        {
            if (flag.Length == 0)
                return;
            var sep = flag.Split(';');
            for (int i = 0; i < sep.Length; i++)
            {
                checker.RegisterFlagObtainment(sep[i]);
            }
        }
    }
}
