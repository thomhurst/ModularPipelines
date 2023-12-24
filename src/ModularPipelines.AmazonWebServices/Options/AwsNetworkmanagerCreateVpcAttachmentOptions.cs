using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-vpc-attachment")]
public record AwsNetworkmanagerCreateVpcAttachmentOptions(
[property: CommandSwitch("--core-network-id")] string CoreNetworkId,
[property: CommandSwitch("--vpc-arn")] string VpcArn,
[property: CommandSwitch("--subnet-arns")] string[] SubnetArns
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}