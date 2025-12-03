using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "list-nodes")]
public record AwsPanoramaListNodesOptions : AwsOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--package-name")]
    public string? PackageName { get; set; }

    [CliOption("--package-version")]
    public string? PackageVersion { get; set; }

    [CliOption("--patch-version")]
    public string? PatchVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}