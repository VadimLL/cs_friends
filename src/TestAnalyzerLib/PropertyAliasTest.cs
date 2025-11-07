using FriendLib;

namespace TestAnalyzerLib;

file class Me
{
    public const string AcceptMul = nameof(AcceptMul);
    public const string AcceptOne = nameof(AcceptOne);

    //[OnlyAlias(typeof(MyFriend), AcceptOne)]
    [OnlyAlias<MyFriend>(AcceptMul)]
    public decimal Money { get; set; } = 100;
}

file class MyFriend
{
    //[FriendAlias(Me.AcceptOne)]
    public void AcceptMoney(in Me me) => me.Money = 0; // err

    [FriendAlias(Me.AcceptMul)]
    public void AcceptMoney(in Me me, int mul) => me.Money = 0;  // ok
}
