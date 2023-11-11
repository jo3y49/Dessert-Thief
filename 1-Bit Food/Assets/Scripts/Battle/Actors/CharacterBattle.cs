using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class CharacterBattle : MonoBehaviour {
    [SerializeField] protected CharacterAnimation characterAnimation;
    public string CharacterName { get; protected set; }
    public int maxHealth;
    public int health;
    public int attack;
    public int defense;
    public float accuracy;
    public float evasion;
    public bool hitTarget = false;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip attackClip;

    protected List<string> attackKeys = new();

    protected List<AttackAction> attackActions = new();

    protected List<int> attackActionUses = new();

    protected virtual void Start() {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    protected static List<AttackAction> FillAttackList(List<string> keys)
    {
        List<AttackAction> attackActions = new();
        AttackList attackList = AttackList.GetInstance();

        foreach (string k in keys)
        {
            attackActions.Add(attackList.GetAction(k));
        }

        return attackActions;
    }

    public virtual void Kill(){}

    public virtual void Defeated(){}

    public virtual void PrepareCombat(){}

    public virtual AttackAction GetAction(int i)
    {
        if (i < attackActions.Count) return attackActions[i];

        else return attackActions[0];
    }

    public virtual int GetActionUses(int i)
    {
        if (i < attackActionUses.Count && i < attackActions.Count) return attackActionUses[i];

        else return 0;
    }

    public virtual int GetActionUses(AttackAction attackAction)
    {
        int i = attackActions.FindIndex(item => item == attackAction);

        return GetActionUses(i);
    }

    public virtual bool CanUseAction(AttackAction attackAction)
    {
        return GetActionUses(attackAction) > 0;
    }

    public virtual int CountActions()
    {
        return attackActions.Count;
    }

    public virtual bool DoAction(AttackAction action, CharacterBattle target)
    {
        return action.Action(this, target);
    }

    public virtual void PlayAttackSound()
    {
        if (hitTarget)
        {
            audioSource.clip = attackClip;
            audioSource.time = 2f;
            audioSource.Play();
        }
    }

    public virtual void Attacked(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public virtual void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
    }

    public virtual void SetAnimationTrigger(string triggerName) { characterAnimation.AnimationTrigger(triggerName); }

    public virtual void SetAnimationBool(string triggerName, bool b) { characterAnimation.AnimationSetBool(triggerName, b); }

    public virtual void AttackTrigger(string triggerName) { characterAnimation.AttackTrigger(triggerName); }

    public virtual Animator GetAnimator() {return characterAnimation.GetAnimator();}

    public virtual bool GetIsAttacking() {return characterAnimation.isAttacking;}

    public virtual void Recover() {}
}