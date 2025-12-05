using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "describe-app-version-app-component")]
public record AwsResiliencehubDescribeAppVersionAppComponentOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-version")] string AppVersion,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}