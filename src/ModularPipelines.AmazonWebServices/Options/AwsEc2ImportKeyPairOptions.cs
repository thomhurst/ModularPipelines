using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "import-key-pair")]
public record AwsEc2ImportKeyPairOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--public-key-material")] string PublicKeyMaterial
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}