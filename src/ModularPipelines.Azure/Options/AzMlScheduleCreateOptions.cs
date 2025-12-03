using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "schedule", "create")]
public record AzMlScheduleCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}