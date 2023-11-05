using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class PFighter : Fighter
{
    [SerializeField] private List<BodyPart> bodyParts = new List<BodyPart>();

    public List<BodyPart> BodyParts { get => bodyParts; }
    public override int Power()
    {
        int power = base.Power();

        foreach(BodyPart part in bodyParts)
        {
            if(part.limb == BodyPart.LimbType.LeftArm && part.LimbIsBroken)
            {
                power -= part.LimbModifier;
            }
            if (part.limb == BodyPart.LimbType.RightArm && part.LimbIsBroken)
            {
                power -= part.LimbModifier;
            }
        }
        return power;
    }

    public override int Defense()
    {
        int defense = base.Defense();

        foreach(BodyPart part in bodyParts)
        {
            if(part.limb == BodyPart.LimbType.LeftLeg && part.LimbIsBroken)
            {
                defense -= part.LimbModifier;
            }
            if(part.limb == BodyPart.LimbType.RightLeg && part.LimbIsBroken)
            {
                defense -= part.LimbModifier;
            }
        }
        return defense;
    }

    public void LowerMaxHealth()
    {
        foreach(BodyPart part in bodyParts)
        {
            if(part.limb == BodyPart.LimbType.Torso && part.LimbIsBroken)
            {
                GetComponent<PFighter>().MaxHp -= part.LimbModifier * 2;
            }
        }
    }

    private void Start()
    {
        foreach(BodyPart part in bodyParts)
        {
            UIManager.instance.SetHealthMax(MaxHp);
            UIManager.instance.SetHealth(Hp, MaxHp);
            UIManager.instance.SetLimbMaxHealth(part.LimbHP, part);
            UIManager.instance.SetLimbHealth(part.LimbHP, part.LimbMaxHP, part);
        }
    }


    public PFighterState PSaveState()
    {
        PFighterState pFighterState = new PFighterState(bodyParts: bodyParts.ConvertAll(x => x.SaveState()));

        return pFighterState;
    }

    public void LoadState(PFighterState state)
    {
        //bodyParts = state.BodyParts;
        int limbNumber = 0;
        foreach(BodyPartState partState in state.BodyParts)
        {
            bodyParts.Add(GetComponentInChildren<BodyPart>());
            bodyParts[limbNumber].LoadState(state.BodyParts[limbNumber]);
            limbNumber++;
        }
    }

    /*
    private IEnumerator LoadPartStates(List<BodyPartState> partStates)
    {
        int partState = 0;
        yield return new WaitForEndOfFrame();

        while (partState < partStates.BodyParts.Count)
        {

        }
    }
    */
}

public class PFighterState
{
    [SerializeField]private List<BodyPartState> bodyParts;

    public List<BodyPartState> BodyParts { get => bodyParts; set => bodyParts = value;}
    public PFighterState(List<BodyPartState> bodyParts)
    {
        this.bodyParts = bodyParts;
    }
}
