using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "update-vpc-attachment")]
public record AwsNetworkmanagerUpdateVpcAttachmentOptions(
[property: CliOption("--attachment-id")] string AttachmentId
) : AwsOptions
{
    [CliOption("--add-subnet-arns")]
    public string[]? AddSubnetArns { get; set; }

    [CliOption("--remove-subnet-arns")]
    public string[]? RemoveSubnetArns { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}