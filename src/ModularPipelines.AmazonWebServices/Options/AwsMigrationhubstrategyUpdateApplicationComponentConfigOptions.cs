using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "update-application-component-config")]
public record AwsMigrationhubstrategyUpdateApplicationComponentConfigOptions(
[property: CliOption("--application-component-id")] string ApplicationComponentId
) : AwsOptions
{
    [CliOption("--app-type")]
    public string? AppType { get; set; }

    [CliOption("--inclusion-status")]
    public string? InclusionStatus { get; set; }

    [CliOption("--secrets-manager-key")]
    public string? SecretsManagerKey { get; set; }

    [CliOption("--source-code-list")]
    public string[]? SourceCodeList { get; set; }

    [CliOption("--strategy-option")]
    public string? StrategyOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}