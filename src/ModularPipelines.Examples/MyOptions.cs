using ModularPipelines.Attributes;

namespace ModularPipelines.Examples;

public class MyOptions
{
    [SecretValue]
    public string MySecret { get; set; } = "Super Secret!";
}