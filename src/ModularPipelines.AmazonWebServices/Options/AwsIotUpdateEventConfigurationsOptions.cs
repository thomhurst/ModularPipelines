using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-event-configurations")]
public record AwsIotUpdateEventConfigurationsOptions : AwsOptions
{
    [CommandSwitch("--event-configurations")]
    public IEnumerable<KeyValue>? EventConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}