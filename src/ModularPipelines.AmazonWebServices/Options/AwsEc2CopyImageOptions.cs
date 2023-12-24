using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "copy-image")]
public record AwsEc2CopyImageOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source-image-id")] string SourceImageId,
[property: CommandSwitch("--source-region")] string SourceRegion
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--destination-outpost-arn")]
    public string? DestinationOutpostArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}