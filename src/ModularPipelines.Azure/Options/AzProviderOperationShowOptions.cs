using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("provider", "operation", "show")]
public record AzProviderOperationShowOptions(
[property: CliOption("--namespace")] string Namespace
) : AzOptions;