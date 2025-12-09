using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "operation", "create")]
public record AzProviderhubOperationCreateOptions(
[property: CliOption("--contents")] string Contents,
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;