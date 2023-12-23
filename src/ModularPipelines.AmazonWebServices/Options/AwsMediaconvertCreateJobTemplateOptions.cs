using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconvert", "create-job-template")]
public record AwsMediaconvertCreateJobTemplateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--settings")] string Settings
) : AwsOptions
{
    [CommandSwitch("--acceleration-settings")]
    public string? AccelerationSettings { get; set; }

    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--hop-destinations")]
    public string[]? HopDestinations { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--queue")]
    public string? Queue { get; set; }

    [CommandSwitch("--status-update-interval")]
    public string? StatusUpdateInterval { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}