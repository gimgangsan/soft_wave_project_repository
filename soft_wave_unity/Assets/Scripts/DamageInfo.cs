

public struct DamageInfo
{
    public int ReducedHP;
    public int ReducedAmour;
    public float HP_Ratio;

    public DamageInfo(int ReducedHP, int ReducedAmour, float HP_Ratio)
    {
        this.ReducedHP = ReducedHP;
        this.ReducedAmour = ReducedAmour;
        this.HP_Ratio = HP_Ratio;
    }

    public DamageInfo(int ReducedHP, int ReducedAmour, int MaxHP, int RemainingHP)
    {
        this.ReducedHP = ReducedHP;
        this.ReducedAmour = ReducedAmour;
        this.HP_Ratio = MaxHP / RemainingHP;
    }
}