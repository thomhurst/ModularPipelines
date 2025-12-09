using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "put-portfolio-preferences")]
public record AwsMigrationhubstrategyPutPortfolioPreferencesOptions : AwsOptions
{
    [CliOption("--application-mode")]
    public string? ApplicationMode { get; set; }

    [CliOption("--application-preferences")]
    public string? ApplicationPreferences { get; set; }

    [CliOption("--database-preferences")]
    public string? DatabasePreferences { get; set; }

    [CliOption("--prioritize-business-goals")]
    public string? PrioritizeBusinessGoals { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}