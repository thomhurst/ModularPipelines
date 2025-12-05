using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "ref", "delete")]
public record AzReposRefDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--object-id")]
    public string? ObjectId { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}