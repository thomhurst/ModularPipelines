using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("translate", "start-text-translation-job")]
public record AwsTranslateStartTextTranslationJobOptions(
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--source-language-code")] string SourceLanguageCode,
[property: CliOption("--target-language-codes")] string[] TargetLanguageCodes
) : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--terminology-names")]
    public string[]? TerminologyNames { get; set; }

    [CliOption("--parallel-data-names")]
    public string[]? ParallelDataNames { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}