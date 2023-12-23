using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-reserved-instances-exchange-quote")]
public record AwsEc2GetReservedInstancesExchangeQuoteOptions(
[property: CommandSwitch("--reserved-instance-ids")] string[] ReservedInstanceIds
) : AwsOptions
{
    [CommandSwitch("--target-configurations")]
    public string[]? TargetConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}