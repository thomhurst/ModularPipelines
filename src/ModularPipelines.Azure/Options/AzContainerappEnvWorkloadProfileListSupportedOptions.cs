using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "env", "workload-profile", "list-supported")]
public record AzContainerappEnvWorkloadProfileListSupportedOptions(
[property: CliOption("--location")] string Location
) : AzOptions;