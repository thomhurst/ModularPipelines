using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "operation", "list")]
public record AzProviderhubOperationListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;