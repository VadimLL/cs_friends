using FriendLib;

namespace TestAnalyzerLib;

file class Me
{
    public const string AcceptMul = nameof(AcceptMul);
    public const string AcceptOne = nameof(AcceptOne);
    public decimal Money { get; private set; } = 100;

    [OnlyAlias(typeof(MyFriend), AcceptMul, AcceptOne)]
    //[OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
    public decimal TakeMyHalfMoney() {
        decimal half = Money / 2;
        Money -= half;
        return half;
    }
}

file class MyFriend
{
    //[FriendAlias(Me.AcceptOne)]
    public void AcceptMoney(in Me me) => Money += me.TakeMyHalfMoney(); // err

    [FriendAlias(Me.AcceptMul)]
    public void AcceptMoney(in Me me, int mul) => Money += mul * me.TakeMyHalfMoney(); // ok
    public decimal Money { get; private set; } = -40;
}
