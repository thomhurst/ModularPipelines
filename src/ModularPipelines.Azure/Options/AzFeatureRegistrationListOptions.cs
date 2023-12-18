using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "registration", "list")]
public record AzFeatureRegistrationListOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }
}

