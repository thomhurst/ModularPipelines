using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "delete-audience-generation-job")]
public record AwsCleanroomsmlDeleteAudienceGenerationJobOptions(
[property: CommandSwitch("--audience-generation-job-arn")] string AudienceGenerationJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}