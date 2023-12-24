using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migration-hub-refactor-spaces", "delete-application")]
public record AwsMigrationHubRefactorSpacesDeleteApplicationOptions(
[property: CommandSwitch("--application-identifier")] string ApplicationIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}