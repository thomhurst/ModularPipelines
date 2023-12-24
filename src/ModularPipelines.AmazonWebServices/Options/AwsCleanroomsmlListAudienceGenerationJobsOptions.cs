using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "list-audience-generation-jobs")]
public record AwsCleanroomsmlListAudienceGenerationJobsOptions : AwsOptions
{
    [CommandSwitch("--collaboration-id")]
    public string? CollaborationId { get; set; }

    [CommandSwitch("--configured-audience-model-arn")]
    public string? ConfiguredAudienceModelArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}