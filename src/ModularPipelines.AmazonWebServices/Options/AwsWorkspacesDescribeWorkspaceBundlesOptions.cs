using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-workspace-bundles")]
public record AwsWorkspacesDescribeWorkspaceBundlesOptions : AwsOptions
{
    [CliOption("--bundle-ids")]
    public string[]? BundleIds { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}