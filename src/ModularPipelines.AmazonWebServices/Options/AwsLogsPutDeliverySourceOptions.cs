using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-delivery-source")]
public record AwsLogsPutDeliverySourceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--log-type")] string LogType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}