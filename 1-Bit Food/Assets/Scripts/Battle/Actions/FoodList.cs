using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodList
{
    private static FoodList instance;
    private Food[] foods;
    private IDictionary<string, FoodAction> foodList;

    private FoodList()
    {
        FillDictionary();
    }

    public static FoodList GetInstance()
    {
        instance ??= new FoodList();

        return instance;
    }

    private void FillDictionary()
    {
        foods = Resources.LoadAll<Food>("");

        foodList = new Dictionary<string, FoodAction>();

        foreach (Food food in foods)
        {
            foodList.Add(food.name, new FoodAction(food.name, 
                (self, target, flavor) => Actions.DoAttack(self, target, food.name, food.damage, flavor),
                (self) => Actions.DoHeal(self, food.name, food.heal)));
        }
    }

    public FoodAction GetAction(string key)
    {
        if (foodList.ContainsKey(key)) return foodList[key];

        else return new FoodAction("Null", EmptyAction, EmptyAction);
    }

    public FoodAction GetAction(int index)
    {
        if (foodList.Count > index) return foodList.ElementAt(index).Value;

        else return new FoodAction("Null", EmptyAction, EmptyAction);
    }

    public List<FoodAction> GetAllActions()
    {
        return foodList.Values.ToList();
    }

    public Dessert[] GetDesserts()
    {
        return (Dessert[])foods.OfType<Dessert>();
    }

    public Ingredient[] GetIngredients()
    {
        return (Ingredient[])foods.OfType<Ingredient>();
    }

    public Food[] GetFoods()
    {
        return foods;
    }

    public int GetFoodIndex(Food food)
    {
        return foods.ToList().FindIndex(item => item == food);
    }

    public void EmptyAction(CharacterBattle self, CharacterBattle target, Flavor flavor = null)
    {
        Debug.Log("This action is null");
    }

    public void EmptyAction(CharacterBattle self = null)
    {
        Debug.Log("This action is null");
    }
}