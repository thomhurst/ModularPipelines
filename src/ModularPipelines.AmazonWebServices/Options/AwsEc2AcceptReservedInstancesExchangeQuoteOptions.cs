using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "accept-reserved-instances-exchange-quote")]
public record AwsEc2AcceptReservedInstancesExchangeQuoteOptions(
[property: CliOption("--reserved-instance-ids")] string[] ReservedInstanceIds
) : AwsOptions
{
    [CliOption("--target-configurations")]
    public string[]? TargetConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}