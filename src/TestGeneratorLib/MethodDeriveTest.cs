namespace TestGeneratorLib;

file class Me
{
    public decimal Rubles { get; private set; } = 100;

    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
    virtual public decimal TakeMyHalfMoney()
    {
        decimal half = Rubles / 2;
        Rubles -= half;
        return half;
    }

    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney2))]
    public decimal TakeMyHalfMoney2()
    {
        decimal half = Rubles / 2;
        Rubles -= half;
        return half;
    }
}

file class Me2 : Me
{
    public override decimal TakeMyHalfMoney()
    {
        base.TakeMyHalfMoney2(); // maybe need err? !!!
        TakeMyHalfMoney2(); // not emplemented !!! is it necessary?
        return base.TakeMyHalfMoney(); // ok
    }
}

file class MyFriend
{
    public void AcceptMoney(in Me me) => Rubles += me.TakeMyHalfMoney(); // ok
    public void CantAcceptMoney(in Me me) => Rubles += me.TakeMyHalfMoney(); // err

    public void AcceptMoney2(in Me2 me2) => Rubles += me2.TakeMyHalfMoney2(); // ok
    public void CantAcceptMoney2(in Me2 me2) => Rubles += me2.TakeMyHalfMoney2(); // err
    public decimal Rubles { get; private set; } = -40;
}
