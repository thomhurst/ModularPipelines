using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "register-volume")]
public record AwsOpsworksRegisterVolumeOptions(
[property: CliOption("--stack-id")] string StackId
) : AwsOptions
{
    [CliOption("--ec2-volume-id")]
    public string? Ec2VolumeId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}