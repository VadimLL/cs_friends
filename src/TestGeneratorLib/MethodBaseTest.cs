namespace TestGeneratorLib;

file class Me
{
    public decimal Rubles { get; private set; } = 100;
    public decimal Yuans { get; private set; } = 100_000;

    //[OnlyYou(typeof(MyFriend), "AcceptMoney")]

    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
    public decimal TakeMyHalfMoney() {
        decimal half = Rubles / 2;
        Rubles -= half;
        return half;
    }
}

file class MyFriend
{
    public void AcceptMoney(in Me me) => Rubles += me.TakeMyHalfMoney(); // ok
    public void CantAcceptMoney(in Me me) => Rubles += me.TakeMyHalfMoney(); // err
    public void AcceptMoneyFromNotMe(in NotMe me) => Rubles += me.TakeMyHalfMoney(); // ok
    public decimal Rubles { get; private set; } = -40;
}


file class NotMe
{
    public decimal TakeMyHalfMoney() => 0;
}
