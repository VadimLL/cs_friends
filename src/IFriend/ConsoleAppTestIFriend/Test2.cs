namespace ConsoleAppTestIFriend;

static class Test2
{
    public static void Run()
    {
        Console.WriteLine(nameof(Test2));

        var me = new Me();
        var myPoorFriend = new MyFriend();

        log(me, myPoorFriend);
        myPoorFriend.AcceptMoney(me);
        log(me, myPoorFriend);

        Console.WriteLine();

        static void log(in Me me, MyFriend friend) =>
            Console.WriteLine($"me: {me.Rubles}₽; friend: {friend.Rubles}₽");
    }
}

file class Me
{
    public interface IFriend
    {
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
      => Rubles += Me.IFriend.TakeMyHalfMoney(me);

    public decimal Rubles { get; private set; } = -40;
}
