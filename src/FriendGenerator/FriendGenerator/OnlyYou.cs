internal static class OnlyYou
{
    public const string AttributeShortName = "OnlyYou";
    public const string AttributeName = $"{AttributeShortName}Attribute";
    public const string AttributeClass =
$@"
using System;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
public class {AttributeName} : Attribute
{{
  public OnlyYouAttribute(Type targetType, params string[] members) {{}}
}}
";
}

