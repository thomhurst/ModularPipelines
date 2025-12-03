using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "add-on", "list")]
public record AzContainerappAddOnListOptions(
[property: CliOption("--environment")] string Environment,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;