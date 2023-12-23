using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "update-vpc-attachment")]
public record AwsNetworkmanagerUpdateVpcAttachmentOptions(
[property: CommandSwitch("--attachment-id")] string AttachmentId
) : AwsOptions
{
    [CommandSwitch("--add-subnet-arns")]
    public string[]? AddSubnetArns { get; set; }

    [CommandSwitch("--remove-subnet-arns")]
    public string[]? RemoveSubnetArns { get; set; }

    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}