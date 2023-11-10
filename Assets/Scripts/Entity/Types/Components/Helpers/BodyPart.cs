using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D.IK;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private int limbHP, limbMaxHP;
    [SerializeField] private bool limbIsBroken;
    [SerializeField] private int limbModifier;
    public enum LimbType { LeftArm, RightArm, LeftLeg, RightLeg, Head, Torso }
    public LimbType limb;
    public bool LimbIsBroken { get => limbIsBroken; }
    public int LimbModifier { get  => limbModifier; }
    public int LimbHP
    {
        get => limbHP; set
        {
            limbHP = Mathf.Max(0, Mathf.Min(value, limbHP));

            if (GetComponent<Player>())
            {
                UIManager.instance.SetLimbHealth(limbHP, limbMaxHP, this);
            }

            if (limbHP == 0)
            {
                limbIsBroken = true;
            }
            else
            {
                limbIsBroken = false;
            }
        }
    }
    public int LimbMaxHP
    {
        get => limbMaxHP; set
        {
            limbMaxHP = value;
            if (GetComponent<Player>())
            {
               UIManager.instance.SetLimbMaxHealth(limbMaxHP, this);
            }
        }
    }

    private void Start()
    {
        UIManager.instance.SetLimbMaxHealth(limbMaxHP, this);
        UIManager.instance.SetLimbHealth(limbHP, limbMaxHP, this);

    }

    public void Heal(int healAmount)
    {
        limbHP = healAmount;
        if(limbHP > 0)
        {
            limbIsBroken = false;
        }
        UIManager.instance.SetLimbHealth(limbHP, limbMaxHP, this);
    }


    public BodyPartState SaveState() => new BodyPartState(
        limbHP: limbHP,
        limbMaxHP: limbMaxHP,
        limbIsBroken: limbIsBroken
        );

    public void LoadState(BodyPartState state )
    {
        limbHP = state.LimbHP;
        limbMaxHP= state.LimbMaxHP;
        limbIsBroken = state.LimbIsBroken;
    }

}

//wtf is a unityreferenceresolver

public class BodyPartState
{
    [SerializeField] private int limbHP, limbMaxHP;
    [SerializeField] private bool limbIsBroken;

    public int LimbHP { get => limbHP; set => limbHP = value; }
    public int LimbMaxHP { get => limbMaxHP;set => limbMaxHP = value; }
    public bool LimbIsBroken { get => limbIsBroken; set => limbIsBroken = value; }

    public BodyPartState(int limbHP, int limbMaxHP, bool limbIsBroken)
    {
        this.limbHP = limbHP;
        this.limbMaxHP = limbMaxHP;
        this.limbIsBroken = limbIsBroken;
    }
}
