using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "get-migrations")]
public record AwsLexModelsGetMigrationsOptions : AwsOptions
{
    [CliOption("--sort-by-attribute")]
    public string? SortByAttribute { get; set; }

    [CliOption("--sort-by-order")]
    public string? SortByOrder { get; set; }

    [CliOption("--v1-bot-name-contains")]
    public string? V1BotNameContains { get; set; }

    [CliOption("--migration-status-equals")]
    public string? MigrationStatusEquals { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}