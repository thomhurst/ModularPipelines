using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource", "patch")]
public record AzResourcePatchOptions(
[property: CliOption("--properties")] string Properties
) : AzOptions
{
    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--is-full-object")]
    public bool? IsFullObject { get; set; }

    [CliFlag("--latest-include-preview")]
    public bool? LatestIncludePreview { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}