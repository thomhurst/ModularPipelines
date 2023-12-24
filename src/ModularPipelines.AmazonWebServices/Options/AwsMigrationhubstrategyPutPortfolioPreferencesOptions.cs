using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "put-portfolio-preferences")]
public record AwsMigrationhubstrategyPutPortfolioPreferencesOptions : AwsOptions
{
    [CommandSwitch("--application-mode")]
    public string? ApplicationMode { get; set; }

    [CommandSwitch("--application-preferences")]
    public string? ApplicationPreferences { get; set; }

    [CommandSwitch("--database-preferences")]
    public string? DatabasePreferences { get; set; }

    [CommandSwitch("--prioritize-business-goals")]
    public string? PrioritizeBusinessGoals { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}