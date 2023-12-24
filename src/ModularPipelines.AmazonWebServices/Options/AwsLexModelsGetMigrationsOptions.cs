using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "get-migrations")]
public record AwsLexModelsGetMigrationsOptions : AwsOptions
{
    [CommandSwitch("--sort-by-attribute")]
    public string? SortByAttribute { get; set; }

    [CommandSwitch("--sort-by-order")]
    public string? SortByOrder { get; set; }

    [CommandSwitch("--v1-bot-name-contains")]
    public string? V1BotNameContains { get; set; }

    [CommandSwitch("--migration-status-equals")]
    public string? MigrationStatusEquals { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}