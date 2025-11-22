namespace ModularPipelines.Attributes;

/// <summary>
/// Marks a property as containing sensitive information that should be obfuscated in logs.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SecretValueAttribute : Attribute;
