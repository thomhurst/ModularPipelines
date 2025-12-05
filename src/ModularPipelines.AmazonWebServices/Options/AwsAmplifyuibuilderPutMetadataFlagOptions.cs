using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "put-metadata-flag")]
public record AwsAmplifyuibuilderPutMetadataFlagOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--feature-name")] string FeatureName,
[property: CliOption("--body")] string Body
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}