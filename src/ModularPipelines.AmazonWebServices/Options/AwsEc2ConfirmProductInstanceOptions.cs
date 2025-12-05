using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "confirm-product-instance")]
public record AwsEc2ConfirmProductInstanceOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--product-code")] string ProductCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}