using FriendLib;

namespace TestAnalyzerLib;

file class Me
{
    public decimal Money { get; private set; } = 100;

    [OnlyYou(typeof(MyFriend1), nameof(MyFriend1.AcceptMoney))]
    [OnlyYou(typeof(MyFriend2), nameof(MyFriend1.AcceptMoney))]
    public decimal TakeMyHalfMoney() {
        decimal half = Money / 2;
        Money -= half;
        return half;
    }
}

file class MyFriend1
{
    public void AcceptMoney(in Me me) => Money += me.TakeMyHalfMoney(); // ok
    public void CantAcceptMoney(in Me me) => Money += me.TakeMyHalfMoney(); // err
    public decimal Money { get; private set; } = -40;
}

file class MyFriend2
{
    public void AcceptMoney(in Me me) => Money += me.TakeMyHalfMoney(); // ok
    public void CantAcceptMoney(in Me me) => Money += me.TakeMyHalfMoney(); // err
    public decimal Money { get; private set; } = -40;
}
