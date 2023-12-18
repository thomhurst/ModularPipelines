using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "registration", "delete")]
public record AzFeatureRegistrationDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}