using ModularPipelines.Attributes;

namespace ModularPipelines.UnitTests.Models;

public class MyModel
{
    [SecretValue]
    public string MySecret { get; } = "This is a secret value!";

    public string NotASecret { get; } = "This is NOT a secret value!";
}