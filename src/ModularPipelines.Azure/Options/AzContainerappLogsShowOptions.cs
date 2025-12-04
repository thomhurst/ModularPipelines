using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "logs", "show")]
public record AzContainerappLogsShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--container")]
    public string? Container { get; set; }

    [CliFlag("--follow")]
    public bool? Follow { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--replica")]
    public string? Replica { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--tail")]
    public string? Tail { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}