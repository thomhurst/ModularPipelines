using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "list-unsupported-app-version-resources")]
public record AwsResiliencehubListUnsupportedAppVersionResourcesOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-version")] string AppVersion
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--resolution-id")]
    public string? ResolutionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}