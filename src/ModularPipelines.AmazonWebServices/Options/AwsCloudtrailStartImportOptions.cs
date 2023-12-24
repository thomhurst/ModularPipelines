using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "start-import")]
public record AwsCloudtrailStartImportOptions : AwsOptions
{
    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--import-source")]
    public string? ImportSource { get; set; }

    [CommandSwitch("--start-event-time")]
    public long? StartEventTime { get; set; }

    [CommandSwitch("--end-event-time")]
    public long? EndEventTime { get; set; }

    [CommandSwitch("--import-id")]
    public string? ImportId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}