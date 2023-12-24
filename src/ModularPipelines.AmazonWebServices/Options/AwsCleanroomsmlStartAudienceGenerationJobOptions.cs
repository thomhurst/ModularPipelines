using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "start-audience-generation-job")]
public record AwsCleanroomsmlStartAudienceGenerationJobOptions(
[property: CommandSwitch("--configured-audience-model-arn")] string ConfiguredAudienceModelArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--seed-audience")] string SeedAudience
) : AwsOptions
{
    [CommandSwitch("--collaboration-id")]
    public string? CollaborationId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}