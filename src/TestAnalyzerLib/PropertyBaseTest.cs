using FriendLib;

namespace TestAnalyzerLib;

file class Me
{ 
    [OnlyYou(typeof(MyFriend), nameof(MyFriend.SetMoney))]
    public decimal Money { get; set; } = 100;

    public void SelfSetMoney() => Money = 200; // ok
}

file class MyFriend
{
    public void SetMoney(in Me me) // ok
    {
        var half = me.Money / 2;
        me.Money = half;
        me.Money = half;
        me.Money += half;
        me.Money -= half;
        me.Money *= half;
        ++me.Money;
        --me.Money;
        me.Money++;
        me.Money--;
    }

    public void CantSetMoney(in Me me) // err
    {
        var half = me.Money / 2;
        me.Money = half;
        me.Money += half;
        me.Money -= half;
        me.Money *= half;
        ++me.Money;
        --me.Money;
        me.Money++;
        me.Money--;
    }
}