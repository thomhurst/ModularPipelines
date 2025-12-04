using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "operation", "list")]
public record AzProviderhubOperationListOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;