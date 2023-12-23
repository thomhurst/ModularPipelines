using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-fpga-image-attribute")]
public record AwsEc2ModifyFpgaImageAttributeOptions(
[property: CommandSwitch("--fpga-image-id")] string FpgaImageId
) : AwsOptions
{
    [CommandSwitch("--attribute")]
    public string? Attribute { get; set; }

    [CommandSwitch("--operation-type")]
    public string? OperationType { get; set; }

    [CommandSwitch("--user-ids")]
    public string[]? UserIds { get; set; }

    [CommandSwitch("--user-groups")]
    public string[]? UserGroups { get; set; }

    [CommandSwitch("--product-codes")]
    public string[]? ProductCodes { get; set; }

    [CommandSwitch("--load-permission")]
    public string? LoadPermission { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}