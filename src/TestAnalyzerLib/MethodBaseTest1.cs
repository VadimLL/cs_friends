using FriendLib;

namespace TestAnalyzerLib;

file class Me
{
    public decimal Rubles { get; private set; } = 100;
    public decimal Yuans { get; private set; } = 100_000;

    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
    [OnlyYou(typeof(MyFriend2))]
    public decimal TakeMyHalfMoney() {
        decimal half = Rubles / 2;
        Rubles -= half;
        return half;
    }

    public void SelfAcceptMoney() => Rubles += TakeMyHalfMoney(); // ok
}

file class MyFriend
{
    public void AcceptMoney(in Me me) => Rubles += me.TakeMyHalfMoney(); // ok
    public void CantAcceptMoney(in Me me) => Rubles += me.TakeMyHalfMoney(); // err
    public void AcceptMoneyFromLoh(in NotMe me) => Rubles += me.TakeMyHalfMoney(); // ok
    public decimal Rubles { get; private set; } = -40;
}

file class MyFriend2
{
    public void AcceptMoney1(in Me me) => Rubles += me.TakeMyHalfMoney(); // ok
    public void AcceptMoney2(in Me me) => Rubles += me.TakeMyHalfMoney(); // ok
    public decimal Rubles { get; private set; } = -40;
}


file class NotMe // Loh :)
{
    public decimal TakeMyHalfMoney() => 1000;
    //...
}

//class PublicMe
//{
//    public decimal Rubles { get; private set; } = 100;
//    public decimal Yuans { get; private set; } = 100_000;
//    [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
//    public decimal TakeMyHalfMoney()
//    {
//        decimal half = Rubles / 2;
//        Rubles -= half;
//        return half;
//    }
//}
