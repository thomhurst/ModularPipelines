using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aksarc", "get-credentials")]
public record AzAksarcGetCredentialsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--admin")]
    public bool? Admin { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }
}