using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "update")]
public record AzGroupUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}