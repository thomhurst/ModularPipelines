using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "create-session")]
public record AwsS3apiCreateSessionOptions(
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--session-mode")]
    public string? SessionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}