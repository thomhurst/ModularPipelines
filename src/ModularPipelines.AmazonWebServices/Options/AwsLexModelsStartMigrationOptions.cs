using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "start-migration")]
public record AwsLexModelsStartMigrationOptions(
[property: CliOption("--v1-bot-name")] string V1BotName,
[property: CliOption("--v1-bot-version")] string V1BotVersion,
[property: CliOption("--v2-bot-name")] string V2BotName,
[property: CliOption("--v2-bot-role")] string V2BotRole,
[property: CliOption("--migration-strategy")] string MigrationStrategy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}