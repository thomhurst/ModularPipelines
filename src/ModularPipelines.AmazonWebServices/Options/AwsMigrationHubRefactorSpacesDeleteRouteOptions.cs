using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migration-hub-refactor-spaces", "delete-route")]
public record AwsMigrationHubRefactorSpacesDeleteRouteOptions(
[property: CommandSwitch("--application-identifier")] string ApplicationIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--route-identifier")] string RouteIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}