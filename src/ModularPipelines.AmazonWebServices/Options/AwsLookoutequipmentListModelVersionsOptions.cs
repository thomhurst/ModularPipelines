using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "list-model-versions")]
public record AwsLookoutequipmentListModelVersionsOptions(
[property: CommandSwitch("--model-name")] string ModelName
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--created-at-end-time")]
    public long? CreatedAtEndTime { get; set; }

    [CommandSwitch("--created-at-start-time")]
    public long? CreatedAtStartTime { get; set; }

    [CommandSwitch("--max-model-version")]
    public long? MaxModelVersion { get; set; }

    [CommandSwitch("--min-model-version")]
    public long? MinModelVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}