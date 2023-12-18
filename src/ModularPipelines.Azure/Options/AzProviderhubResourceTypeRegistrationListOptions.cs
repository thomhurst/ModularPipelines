using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "resource-type-registration", "list")]
public record AzProviderhubResourceTypeRegistrationListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

