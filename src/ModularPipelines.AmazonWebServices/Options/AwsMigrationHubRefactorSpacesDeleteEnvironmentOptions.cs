using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migration-hub-refactor-spaces", "delete-environment")]
public record AwsMigrationHubRefactorSpacesDeleteEnvironmentOptions(
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}