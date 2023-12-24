using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "start-audience-export-job")]
public record AwsCleanroomsmlStartAudienceExportJobOptions(
[property: CommandSwitch("--audience-generation-job-arn")] string AudienceGenerationJobArn,
[property: CommandSwitch("--audience-size")] string AudienceSize,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}