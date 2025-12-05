using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-image-attribute")]
public record AwsEc2ModifyImageAttributeOptions(
[property: CliOption("--image-id")] string ImageId
) : AwsOptions
{
    [CliOption("--attribute")]
    public string? Attribute { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--launch-permission")]
    public string? LaunchPermission { get; set; }

    [CliOption("--operation-type")]
    public string? OperationType { get; set; }

    [CliOption("--product-codes")]
    public string[]? ProductCodes { get; set; }

    [CliOption("--user-groups")]
    public string[]? UserGroups { get; set; }

    [CliOption("--user-ids")]
    public string[]? UserIds { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }

    [CliOption("--organization-arns")]
    public string[]? OrganizationArns { get; set; }

    [CliOption("--organizational-unit-arns")]
    public string[]? OrganizationalUnitArns { get; set; }

    [CliOption("--imds-support")]
    public string? ImdsSupport { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}