using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-security")]
public record AwsKafkaUpdateSecurityOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--client-authentication")]
    public string? ClientAuthentication { get; set; }

    [CliOption("--encryption-info")]
    public string? EncryptionInfo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}