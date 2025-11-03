using FriendLib;

namespace TestAnalyzerLib
{
    file class Me
    {
        public decimal Money { get; private set; } = 100;

        [OnlyYou(typeof(TestAnalyzerLib1.MyFriend),
            nameof(TestAnalyzerLib1.MyFriend.AcceptMoney))]
        public decimal TakeMyHalfMoney1()
        {
            decimal half = Money / 2;
            Money -= half;
            return half;
        }

        [OnlyYou(typeof(TestAnalyzerLib2.MyFriend),
            nameof(TestAnalyzerLib2.MyFriend.AcceptMoney))]
        public decimal TakeMyHalfMoney2()
        {
            decimal half = Money / 2;
            Money -= half;
            return half;
        }
    }
}

namespace TestAnalyzerLib1
{
    using TestAnalyzerLib;
    file class MyFriend
    {
        public void AcceptMoney(in Me me)
        {
            Money += me.TakeMyHalfMoney1(); // ok
            Money += me.TakeMyHalfMoney2(); // err
        }
        public void CantAcceptMoney(in Me me) => Money += me.TakeMyHalfMoney1(); // err
        public decimal Money { get; private set; } = -40;
    }
}

namespace TestAnalyzerLib2
{
    using TestAnalyzerLib;
    file class MyFriend
    {
        public void AcceptMoney(in Me me)
        {
            Money += me.TakeMyHalfMoney1(); // err
            Money += me.TakeMyHalfMoney2(); // ok
        }
        public void CantAcceptMoney(in Me me) => Money += me.TakeMyHalfMoney2(); // err
        public decimal Money { get; private set; } = -40;
    }
}