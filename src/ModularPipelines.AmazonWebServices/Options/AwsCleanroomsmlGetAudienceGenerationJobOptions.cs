using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "get-audience-generation-job")]
public record AwsCleanroomsmlGetAudienceGenerationJobOptions(
[property: CommandSwitch("--audience-generation-job-arn")] string AudienceGenerationJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}