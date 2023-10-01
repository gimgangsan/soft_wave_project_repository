

using UnityEngine;

public struct DamageInfo
{
   
    public int ReducedHP;
    public int ReducedAmour;
    //public float HP_Ratio;
}

public struct HealInfo
{
    public int HealedHP;
    public int HealedAmour;
    //public float HP_Ratio;
}

public struct HPInfo
{
    public int MaxHP;
    public int HP;
    public int Amour;

    public readonly float GetHPRatio()
    {
        return (float)HP/ (float)MaxHP;
    }
}
