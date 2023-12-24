using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "describe-broker-instance-options")]
public record AwsMqDescribeBrokerInstanceOptionsOptions : AwsOptions
{
    [CommandSwitch("--engine-type")]
    public string? EngineType { get; set; }

    [CommandSwitch("--host-instance-type")]
    public string? HostInstanceType { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}