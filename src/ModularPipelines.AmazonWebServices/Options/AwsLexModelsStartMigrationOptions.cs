using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "start-migration")]
public record AwsLexModelsStartMigrationOptions(
[property: CommandSwitch("--v1-bot-name")] string V1BotName,
[property: CommandSwitch("--v1-bot-version")] string V1BotVersion,
[property: CommandSwitch("--v2-bot-name")] string V2BotName,
[property: CommandSwitch("--v2-bot-role")] string V2BotRole,
[property: CommandSwitch("--migration-strategy")] string MigrationStrategy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}