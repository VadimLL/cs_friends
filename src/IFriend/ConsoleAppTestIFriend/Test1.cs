namespace ConsoleAppTestIFriend;

static class Test1
{
    public static void Run()
    {
        Console.WriteLine(nameof(Test1));

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
        // Setter
        static protected void setMoney(in Me self, in decimal value)
          => self.Rubles = value;
    }

    public decimal Rubles { get; private set; } = 100;
    public decimal Yuans { get; private set; } = 100_000;
}

file class MyFriend : Me.IFriend
{
    public void AcceptMoney(in Me me)
    {
        decimal half = me.Rubles / 2;
        Me.IFriend.setMoney(me, half);
        Rubles += half;
    }

    public decimal Rubles { get; private set; } = -40;
}
