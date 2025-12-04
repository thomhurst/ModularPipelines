using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "exec")]
public record AzContainerappExecOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--replica")]
    public string? Replica { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }
}