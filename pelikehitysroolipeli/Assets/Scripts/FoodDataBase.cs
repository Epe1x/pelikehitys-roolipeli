public static class FoodDatabase
{
    public static int GetFoodPrice(FoodType food)
    {
        switch (food)
        {
            case FoodType.Kasvispasta: return 10;
            case FoodType.Kanakeitto: return 20;
            case FoodType.Pihviateria: return 35;
        }
        return 0;
    }

    public static int GetFoodHeal(FoodType food)
    {
        switch (food)
        {
            case FoodType.Kasvispasta: return 10;
            case FoodType.Kanakeitto: return 25;
            case FoodType.Pihviateria: return 45;
        }
        return 0;
    }
}