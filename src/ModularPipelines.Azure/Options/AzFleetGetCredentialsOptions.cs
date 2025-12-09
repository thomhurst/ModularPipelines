using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "get-credentials")]
public record AzFleetGetCredentialsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }
}