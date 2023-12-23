using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "get-configured-audience-model")]
public record AwsCleanroomsmlGetConfiguredAudienceModelOptions(
[property: CommandSwitch("--configured-audience-model-arn")] string ConfiguredAudienceModelArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}