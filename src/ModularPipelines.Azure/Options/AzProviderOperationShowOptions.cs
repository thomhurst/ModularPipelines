using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "operation", "show")]
public record AzProviderOperationShowOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions;