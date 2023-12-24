using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "start-recommendations")]
public record AwsDmsStartRecommendationsOptions(
[property: CommandSwitch("--database-id")] string DatabaseId,
[property: CommandSwitch("--settings")] string Settings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}