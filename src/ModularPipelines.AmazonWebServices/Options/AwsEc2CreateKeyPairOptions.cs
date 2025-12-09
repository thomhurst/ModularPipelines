using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-key-pair")]
public record AwsEc2CreateKeyPairOptions(
[property: CliOption("--key-name")] string KeyName
) : AwsOptions
{
    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--key-format")]
    public string? KeyFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}