using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattle : CharacterBattle {    

    protected List<int> actionUses = new();
    private Dictionary<string, int> flavorUses = new();

    protected List<PlayerAction> actions = new();

    protected override void Start() {
        base.Start();

        health = GameManager.instance.GetHealth();

        uiManager.SetHealth();

        CharacterName = "Player";

        actions = FoodList.GetInstance().GetAllActions();

        actionUses = GameManager.instance.GetFoodUsesList();

        List<int> flavorList = GameManager.instance.GetFlavorUsesList();

        flavorUses = new Dictionary<string, int>
        {
            {"Vanilla", flavorList[0]},
            {"Chocolate", flavorList[1]},
            {"Strawberry", flavorList[2]},
            {"Lemon", flavorList[3]},
            {"Mint", flavorList[4]},
        };
    }

    public override void Attacked(int damage, Flavor flavor = null)
    {
        base.Attacked(damage);

        uiManager.LoseHealth(damage);

        // GameManager.instance.AddHealth(-damage);
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);

        uiManager.AddHealth(heal);

        // GameManager.instance.AddHealth(heal);
    }

    public override void PrepareCombat()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void EndCombat(int money)
    {
        GetComponent<PlayerMovement>().enabled = true;

        GameManager.instance.AddPlayerMoney(money);
        GameManager.instance.SetFoodUses(actionUses);
        GameManager.instance.SetFlavorUses(flavorUses.Values.ToList());

        GameManager.instance.SetHealth(health);
    }

    public override Food StolenItem(Food food)
    {
        if (food != null)
        {
            actionUses[food.index]--;
            return food;
        }

        int r = Random.Range(0, actionUses.Count);

        foreach (int i in actionUses)
        {
            if (actionUses[r] > 0)
            {
                actionUses[r]--;
                return FoodList.GetInstance().GetFoods()[r];
                
            } else 
            {
                if (r < actionUses.Count - 1)
                    r++;
                else
                    r = 0;
            }
        }

        return null;
    }

    public override void StealItem(Food food)
    {
        actionUses[food.index]++;
    }

    public bool OutOfFood()
    {
        return actionUses.Max() <= 0;
    }

    public void ResetHealth()
    {
        health = maxHealth;

        GameManager.instance.SetHealth(maxHealth);
    }

    public virtual PlayerAction GetAction(int i)
    {
        if (i < actions.Count) return actions[i];

        else return actions[0];
    }

    public virtual int GetActionUses(int i)
    {
        if (i < actionUses.Count && i < actions.Count) return actionUses[i];

        else return 0;
    }

    public virtual int GetActionUses(PlayerAction action)
    {
        int i = actions.FindIndex(item => item == action);

        return GetActionUses(i);
    }

    public virtual int GetActionUses(Food food)
    {
        return GetActionUses(food.index);
    }

    public virtual int GetFlavorUses(int i)
    {
        if (i < flavorUses.Count) return flavorUses.ElementAt(i).Value;

        else return 0;
    }

    public virtual bool CanUseAction(PlayerAction attackAction)
    {
        return GetActionUses(attackAction) > 0;
    }

    public override string DoAttack(CharacterAction action, CharacterBattle target, Flavor flavor = null)
    {
        UseAction(action, flavor);

        if (action.GetType() == typeof(PlayerAction))
            (action as PlayerAction).Attack(this, target, flavor);

        return "";
    }

    public virtual void DoHeal(CharacterAction action, Flavor flavor = null)
    {
        UseAction(action, flavor);

        (action as PlayerAction).Heal(this, flavor);
    }

    protected void UseAction(CharacterAction action, Flavor flavor = null)
    {
        int i = actions.FindIndex(item => item == action);

        if (i != -1 && actionUses.Count > i)
        {
            actionUses[i]--;
            if (flavor != null && flavorUses.ContainsKey(flavor.name))
            {
                flavorUses[flavor.name]--;
            }
        }
    }
    

    public virtual int CountActions()
    {
        return actions.Count;
    }

    public void RegetUses()
    {
        actionUses = GameManager.instance.GetFoodUsesList();
    }
}