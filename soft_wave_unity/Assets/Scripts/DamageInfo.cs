

using UnityEngine;

public struct DamageInfo
{
    public int ReducedHP;
    public int ReducedAmour;
}

public struct HealInfo
{
    public int HealedHP;
    public int HealedAmour;
}

public struct HPInfo
{
    public int MaxHP;
    public int HP;
    public int Amour;

    public readonly float GetHPRatio()
    {
        if(HP + Amour <= MaxHP) { return (float)HP / (float)MaxHP; }
        else { return (float)HP / (float)(HP + Amour); }
    }

    public readonly float GetAmourRatio()
    {
        if (HP + Amour <= MaxHP) { return (float)Amour / (float)MaxHP; }
        else { return (float)Amour / (float)(HP + Amour); }
    }
}
