using System;

namespace FriendLib;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class FriendAliasAttribute : Attribute
{
    public FriendAliasAttribute(string alias) { }
}
