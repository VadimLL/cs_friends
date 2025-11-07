using System;

namespace FriendLib;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
public class OnlyAliasAttribute : Attribute
{
    public OnlyAliasAttribute(Type type, params string[] aliases) { }
}

// possible variant for C# 11 and above:
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
public class OnlyAliasAttribute<T> : Attribute
{
    public OnlyAliasAttribute(params string[] aliases) { }
}
