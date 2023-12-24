using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-image-attribute")]
public record AwsEc2ModifyImageAttributeOptions(
[property: CommandSwitch("--image-id")] string ImageId
) : AwsOptions
{
    [CommandSwitch("--attribute")]
    public string? Attribute { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--launch-permission")]
    public string? LaunchPermission { get; set; }

    [CommandSwitch("--operation-type")]
    public string? OperationType { get; set; }

    [CommandSwitch("--product-codes")]
    public string[]? ProductCodes { get; set; }

    [CommandSwitch("--user-groups")]
    public string[]? UserGroups { get; set; }

    [CommandSwitch("--user-ids")]
    public string[]? UserIds { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }

    [CommandSwitch("--organization-arns")]
    public string[]? OrganizationArns { get; set; }

    [CommandSwitch("--organizational-unit-arns")]
    public string[]? OrganizationalUnitArns { get; set; }

    [CommandSwitch("--imds-support")]
    public string? ImdsSupport { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}