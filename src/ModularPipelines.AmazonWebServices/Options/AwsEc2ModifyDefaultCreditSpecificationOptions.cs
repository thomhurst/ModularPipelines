using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-default-credit-specification")]
public record AwsEc2ModifyDefaultCreditSpecificationOptions(
[property: CliOption("--instance-family")] string InstanceFamily,
[property: CliOption("--cpu-credits")] string CpuCredits
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}