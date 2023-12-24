using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "delete-fleet-advisor-databases")]
public record AwsDmsDeleteFleetAdvisorDatabasesOptions(
[property: CommandSwitch("--database-ids")] string[] DatabaseIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}