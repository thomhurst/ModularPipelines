using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "operation", "create")]
public record AzProviderhubOperationCreateOptions(
[property: CommandSwitch("--contents")] string Contents,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;