using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "delete-fleet-advisor-collector")]
public record AwsDmsDeleteFleetAdvisorCollectorOptions(
[property: CommandSwitch("--collector-referenced-id")] string CollectorReferencedId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}