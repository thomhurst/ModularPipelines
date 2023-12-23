using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "put-logging-options")]
public record AwsIotfleetwisePutLoggingOptionsOptions(
[property: CommandSwitch("--cloud-watch-log-delivery")] string CloudWatchLogDelivery
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}