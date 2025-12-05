using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "refresh-token")]
public record AwsAmplifyuibuilderRefreshTokenOptions(
[property: CliOption("--provider")] string Provider,
[property: CliOption("--refresh-token-body")] string RefreshTokenBody
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}