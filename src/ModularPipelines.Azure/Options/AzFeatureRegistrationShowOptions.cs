using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "registration", "show")]
public record AzFeatureRegistrationShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
}

