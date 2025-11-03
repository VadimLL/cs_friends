using FriendLib;

namespace TestAnalyzerLib;

file class Me
{
    public interface IFriend
    {
        [OnlyYou(typeof(MyFriend), nameof(MyFriend.AcceptMoney))]
        static decimal TakeMyHalfMoney(Me self)
        {
            decimal half = self.Rubles / 2;
            self.Rubles -= half;
            return half;
        }
    }

    public decimal Rubles { get; private set; } = 100;
    public decimal Yuans { get; private set; } = 100_000;
}

file class MyFriend : Me.IFriend
{
    public void AcceptMoney(in Me me)
      => Rubles += Me.IFriend.TakeMyHalfMoney(me); // ok

    public void CantAcceptMoney(in Me me)
      => Rubles += Me.IFriend.TakeMyHalfMoney(me); // err

    public decimal Rubles { get; private set; } = -40;
}
