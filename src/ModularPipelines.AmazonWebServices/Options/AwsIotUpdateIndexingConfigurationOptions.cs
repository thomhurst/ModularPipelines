using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-indexing-configuration")]
public record AwsIotUpdateIndexingConfigurationOptions : AwsOptions
{
    [CommandSwitch("--thing-indexing-configuration")]
    public string? ThingIndexingConfiguration { get; set; }

    [CommandSwitch("--thing-group-indexing-configuration")]
    public string? ThingGroupIndexingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}