using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "get-service")]
public record AwsMigrationHubRefactorSpacesGetServiceOptions(
[property: CliOption("--application-identifier")] string ApplicationIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}