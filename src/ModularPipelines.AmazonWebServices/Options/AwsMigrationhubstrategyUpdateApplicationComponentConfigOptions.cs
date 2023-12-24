using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "update-application-component-config")]
public record AwsMigrationhubstrategyUpdateApplicationComponentConfigOptions(
[property: CommandSwitch("--application-component-id")] string ApplicationComponentId
) : AwsOptions
{
    [CommandSwitch("--app-type")]
    public string? AppType { get; set; }

    [CommandSwitch("--inclusion-status")]
    public string? InclusionStatus { get; set; }

    [CommandSwitch("--secrets-manager-key")]
    public string? SecretsManagerKey { get; set; }

    [CommandSwitch("--source-code-list")]
    public string[]? SourceCodeList { get; set; }

    [CommandSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}