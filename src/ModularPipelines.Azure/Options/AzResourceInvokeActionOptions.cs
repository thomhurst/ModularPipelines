using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource", "invoke-action")]
public record AzResourceInvokeActionOptions(
[property: CliOption("--action")] string Action
) : AzOptions
{
    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--latest-include-preview")]
    public bool? LatestIncludePreview { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--request-body")]
    public string? RequestBody { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}