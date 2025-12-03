using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "list-assets")]
public record AwsOutpostsListAssetsOptions(
[property: CliOption("--outpost-identifier")] string OutpostIdentifier
) : AwsOptions
{
    [CliOption("--host-id-filter")]
    public string[]? HostIdFilter { get; set; }

    [CliOption("--status-filter")]
    public string[]? StatusFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}