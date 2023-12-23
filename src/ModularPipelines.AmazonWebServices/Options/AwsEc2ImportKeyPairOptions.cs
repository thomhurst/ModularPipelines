using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "import-key-pair")]
public record AwsEc2ImportKeyPairOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--public-key-material")] string PublicKeyMaterial
) : AwsOptions
{
    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}