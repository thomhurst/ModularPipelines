using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-key-pair")]
public record AwsEc2CreateKeyPairOptions(
[property: CommandSwitch("--key-name")] string KeyName
) : AwsOptions
{
    [CommandSwitch("--key-type")]
    public string? KeyType { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--key-format")]
    public string? KeyFormat { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}