public static class ArrowDatabase
{
    public static int GetArrowPrice(ArrowType arrow)
    {
        switch (arrow)
        {
            case ArrowType.Aloittelijanuoli: return 5;
            case ArrowType.Perusnuoli: return 15;
            case ArrowType.Eliittinuoli: return 20;
            default: return 0;
        }
    }

    public static int GetArrowDamage(ArrowType arrow)
    {
        switch (arrow)
        {
            case ArrowType.Aloittelijanuoli: return 10;
            case ArrowType.Perusnuoli: return 25;
            case ArrowType.Eliittinuoli: return 20;
            default: return 0;
        }
    }
}