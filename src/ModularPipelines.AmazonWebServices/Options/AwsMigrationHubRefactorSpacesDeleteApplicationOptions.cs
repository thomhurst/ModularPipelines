using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "delete-application")]
public record AwsMigrationHubRefactorSpacesDeleteApplicationOptions(
[property: CliOption("--application-identifier")] string ApplicationIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}