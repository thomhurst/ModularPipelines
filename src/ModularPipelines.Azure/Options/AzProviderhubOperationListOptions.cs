using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "operation", "list")]
public record AzProviderhubOperationListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
}

