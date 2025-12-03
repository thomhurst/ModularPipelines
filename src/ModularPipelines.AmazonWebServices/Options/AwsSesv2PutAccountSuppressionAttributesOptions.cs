using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-account-suppression-attributes")]
public record AwsSesv2PutAccountSuppressionAttributesOptions : AwsOptions
{
    [CliOption("--suppressed-reasons")]
    public string[]? SuppressedReasons { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}