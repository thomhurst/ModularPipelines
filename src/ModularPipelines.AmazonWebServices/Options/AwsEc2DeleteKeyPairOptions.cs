using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-key-pair")]
public record AwsEc2DeleteKeyPairOptions : AwsOptions
{
    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-pair-id")]
    public string? KeyPairId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}