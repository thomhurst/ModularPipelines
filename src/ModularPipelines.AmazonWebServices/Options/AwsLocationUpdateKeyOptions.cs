using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "update-key")]
public record AwsLocationUpdateKeyOptions(
[property: CliOption("--key-name")] string KeyName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--expire-time")]
    public long? ExpireTime { get; set; }

    [CliOption("--restrictions")]
    public string? Restrictions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}