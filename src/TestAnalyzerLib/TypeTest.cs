using FriendLib;

namespace TestAnalyzerLib;

[OnlyYou(typeof(MyFriend))]
file class Me
{
    public decimal Money { get; set; } = 100;

    public void Method1() { }

    [OnlyYou(typeof(MyFriend), nameof(MyFriend.CanInvoke1))]
    public void Method2() { }
    
    [OnlyYou(typeof(NotMyFriend), nameof(MyFriend.CanInvoke1))]
    [OnlyYou(typeof(NotMyFriend), nameof(NotMyFriend.Some1))] // no effect !!! запретить атрибут правилом? 
    public void Method3() { }
}

file class MyFriend
{
    public void CanInvoke1(in Me me) => me.Method2(); // ok
    public void CanInvoke2(in Me me) => me.Method1(); // ok
    public void CanSet(in Me me) => me.Money = 0; // ok
    public void CantInvoke(in Me me) => me.Method2(); // err
    public void CantInvoke3(in Me me) => me.Method3(); // err??? !!!
}

file class NotMyFriend
{
    public void Some1(in Me me) => me.Method1(); // err
    public void Some2(in Me me) => me.Method2(); // err
    public void Some3(in Me me) => me.Method3(); // err
    public void SomeSet(in Me me) => me.Money = 0; // err
}
