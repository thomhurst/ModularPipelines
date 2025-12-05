using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-fpga-image-attribute")]
public record AwsEc2ModifyFpgaImageAttributeOptions(
[property: CliOption("--fpga-image-id")] string FpgaImageId
) : AwsOptions
{
    [CliOption("--attribute")]
    public string? Attribute { get; set; }

    [CliOption("--operation-type")]
    public string? OperationType { get; set; }

    [CliOption("--user-ids")]
    public string[]? UserIds { get; set; }

    [CliOption("--user-groups")]
    public string[]? UserGroups { get; set; }

    [CliOption("--product-codes")]
    public string[]? ProductCodes { get; set; }

    [CliOption("--load-permission")]
    public string? LoadPermission { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}