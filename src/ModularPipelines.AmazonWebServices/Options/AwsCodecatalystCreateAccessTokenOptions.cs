using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "create-access-token")]
public record AwsCodecatalystCreateAccessTokenOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--expires-time")]
    public long? ExpiresTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}