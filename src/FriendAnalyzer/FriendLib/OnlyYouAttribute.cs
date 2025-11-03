using System;

namespace FriendLib;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property,
    AllowMultiple = true)]
public class OnlyYouAttribute : Attribute
{
    public OnlyYouAttribute(Type type, params string[] members) { }
}
