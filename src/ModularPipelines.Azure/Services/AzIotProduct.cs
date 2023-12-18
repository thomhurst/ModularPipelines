using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotProduct
{
    public AzIotProduct(
        AzIotProductRequirement requirement,
        AzIotProductTest test
    )
    {
        Requirement = requirement;
        Test = test;
    }

    public AzIotProductRequirement Requirement { get; }

    public AzIotProductTest Test { get; }
}