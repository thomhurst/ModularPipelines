using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migration-hub-refactor-spaces", "update-route")]
public record AwsMigrationHubRefactorSpacesUpdateRouteOptions(
[property: CommandSwitch("--activation-state")] string ActivationState,
[property: CommandSwitch("--application-identifier")] string ApplicationIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--route-identifier")] string RouteIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}