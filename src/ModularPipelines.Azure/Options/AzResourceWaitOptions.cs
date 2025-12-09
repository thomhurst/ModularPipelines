using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource", "wait")]
public record AzResourceWaitOptions : AzOptions
{
    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--include-response-body")]
    public bool? IncludeResponseBody { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

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

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}