using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource", "create")]
public record AzResourceCreateOptions(
[property: CliOption("--properties")] string Properties
) : AzOptions
{
    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--is-full-object")]
    public bool? IsFullObject { get; set; }

    [CliFlag("--latest-include-preview")]
    public bool? LatestIncludePreview { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

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