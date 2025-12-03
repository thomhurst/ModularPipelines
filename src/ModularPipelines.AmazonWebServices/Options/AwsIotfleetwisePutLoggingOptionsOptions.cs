using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "put-logging-options")]
public record AwsIotfleetwisePutLoggingOptionsOptions(
[property: CliOption("--cloud-watch-log-delivery")] string CloudWatchLogDelivery
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}