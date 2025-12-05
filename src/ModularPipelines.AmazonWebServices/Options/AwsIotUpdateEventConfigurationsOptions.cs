using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-event-configurations")]
public record AwsIotUpdateEventConfigurationsOptions : AwsOptions
{
    [CliOption("--event-configurations")]
    public IEnumerable<KeyValue>? EventConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}