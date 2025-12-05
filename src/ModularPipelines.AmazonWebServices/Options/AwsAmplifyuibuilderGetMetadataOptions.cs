using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "get-metadata")]
public record AwsAmplifyuibuilderGetMetadataOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}