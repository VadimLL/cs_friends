using System;

namespace FriendLib;

[AttributeUsage(
      AttributeTargets.Class
    | AttributeTargets.Interface
    | AttributeTargets.Method
    | AttributeTargets.Property
    | AttributeTargets.Field
    , AllowMultiple = true)]
public class OnlyYouAttribute : Attribute
{
    public OnlyYouAttribute(Type type, params string[] members) { }
}

// possible variant for C# 11 and above:
[AttributeUsage(
      AttributeTargets.Class
    | AttributeTargets.Interface
    | AttributeTargets.Method
    | AttributeTargets.Property
    | AttributeTargets.Field
    , AllowMultiple = true)]
public class OnlyYouAttribute<T> : Attribute
{
    public OnlyYouAttribute(params string[] members) { }
}
