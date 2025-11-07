namespace TestGeneratorLib;

file interface IMe
{
    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
    decimal TakeMyHalfMoney();

    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
    static void StaticMethod() { }
}

file class Me : IMe
{
    public decimal Rubles { get; private set; } = 100;
    public decimal TakeMyHalfMoney()
    {
        decimal half = Rubles / 2;
        Rubles -= half;
        return half;
    }
}

file class MyFriend
{
    public void AcceptMoney(in Me me)
    {
        Rubles += me.TakeMyHalfMoney(); // ok
        IMe.StaticMethod(); // ok
    }
    public void CantAcceptMoney(in Me me)
    {
        Rubles += me.TakeMyHalfMoney(); // err? not emplemented !!! is it necessary?
        IMe.StaticMethod(); // err
    }
    public decimal Rubles { get; private set; } = -40;
}
