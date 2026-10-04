using UnityEngine;

public struct GearOption
{
    private float baseAttack;
    private float baseHaste;
    private float baseSize;
    private float defense;
    private float hpRegen;

    private static readonly GearOption identityOption = new GearOption(1, 0, 1, 0, 0);
    public static GearOption Identity => identityOption;

    private GearOption(float baseAttack, float baseHaste, float baseSize, float defense, float hpRegen)
    {
        this.baseAttack = baseAttack;
        this.baseHaste = baseHaste;
        this.baseSize = baseSize;
        this.defense = defense;
        this.hpRegen = hpRegen;
    }

    public float BaseAttack => baseAttack;
    public float BaseHaste => baseHaste;
    public float BaseSize => baseSize;
    public float Defense => defense;
    public float HpRegen => hpRegen;

    //public static GearOption operator +(GearOption a, GearOption b)
    //{
    //    GearOption result = new GearOption();
    //    result.baseAttack = a.baseAttack + b.baseAttack;
    //    result.baseHaste = a.baseHaste + b.baseHaste;
    //    result.baseSize = a.baseSize + b.baseSize;
    //    result.defense = a.defense + b.defense;
    //    result.hpRegen = a.HpRegen + b.hpRegen;

    //    return result;
    //}

    public void Apply(Gears type, float value)
    {
        switch (type)
        {
            case Gears.BaseAttack:
                this.baseAttack = value;
                break;
            case Gears.BaseHaste:
                this.baseHaste = value;
                break;
            case Gears.BaseSize:
                this.baseSize = value;
                break;
            case Gears.Defense:
                this.defense = value;
                break;
            case Gears.HpRegen:
                this.hpRegen = value;
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(type));
        }
    }

}