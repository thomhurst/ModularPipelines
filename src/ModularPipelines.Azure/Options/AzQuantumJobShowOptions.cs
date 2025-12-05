using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("quantum", "job", "show")]
public record AzQuantumJobShowOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;