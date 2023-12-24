using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "list-action-types")]
public record AwsCodepipelineListActionTypesOptions : AwsOptions
{
    [CommandSwitch("--action-owner-filter")]
    public string? ActionOwnerFilter { get; set; }

    [CommandSwitch("--region-filter")]
    public string? RegionFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}