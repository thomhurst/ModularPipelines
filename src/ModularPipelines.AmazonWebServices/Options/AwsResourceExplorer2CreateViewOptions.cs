using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-explorer-2", "create-view")]
public record AwsResourceExplorer2CreateViewOptions(
[property: CliOption("--view-name")] string ViewName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--included-properties")]
    public string[]? IncludedProperties { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}