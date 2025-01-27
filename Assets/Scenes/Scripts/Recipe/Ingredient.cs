using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class Ingredient
{
    public enum Base { noCondition, rice, bread, noodle }
    
    public enum Cook { noCondition, none, stirFry, roast }

    public enum Main { noCondition, meat, fish, vege }

    public enum MeatFish { noCondition, none, pork, egg, chicken, beef, salmon = 100 }

    public enum Vege { noCondition, none, potato, tomato, mushroom, carrot }

    // Binding Main Ingredient Category with its ingredients
    private static readonly Dictionary<MeatFish, Main> _meatMapping = new Dictionary<MeatFish, Main>
    {
        {MeatFish.pork, Main.meat },
        {MeatFish.egg, Main.meat },
        {MeatFish.chicken, Main.meat },
        {MeatFish.beef, Main.meat }
    };
    private static readonly Dictionary<MeatFish, Main> _fishMapping = new Dictionary<MeatFish, Main>
    {
        {MeatFish.salmon, Main.fish }
    };
    private static readonly Dictionary<Vege, Main> _vegeMapping = new Dictionary<Vege, Main>
    {
        {Vege.potato, Main.vege },
        {Vege.tomato, Main.vege },
        {Vege.mushroom, Main.vege },
        {Vege.carrot, Main.vege },
    };

    public static bool IsSubCategory(MeatFish meatfish, Main main)
    {
        if (main == Main.meat)
            return _meatMapping.TryGetValue(meatfish, out var mapped) && mapped == main;
        else
            return _fishMapping.TryGetValue(meatfish, out var mapped) && mapped == main;
    }

    public static bool IsSubCategory(Vege vege, Main main)
    {
        return _vegeMapping.TryGetValue(vege, out var mapped) && mapped == main;
    }
}
