using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "delete-route")]
public record AwsMigrationHubRefactorSpacesDeleteRouteOptions(
[property: CliOption("--application-identifier")] string ApplicationIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--route-identifier")] string RouteIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}