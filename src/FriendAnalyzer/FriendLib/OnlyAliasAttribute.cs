using System;

namespace FriendLib;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
public class OnlyAliasAttribute : Attribute
{
    public OnlyAliasAttribute(Type type, params string[] aliases) { }
}
