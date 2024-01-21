namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PositionalArgumentAttribute : Attribute
{
    public Position Position { get; set; } = Position.BeforeSwitches;

    public string? PlaceholderName { get; set; }
}