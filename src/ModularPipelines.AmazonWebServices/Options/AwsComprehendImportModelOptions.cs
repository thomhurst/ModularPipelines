using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "import-model")]
public record AwsComprehendImportModelOptions(
[property: CliOption("--source-model-arn")] string SourceModelArn
) : AwsOptions
{
    [CliOption("--model-name")]
    public string? ModelName { get; set; }

    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--model-kms-key-id")]
    public string? ModelKmsKeyId { get; set; }

    [CliOption("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}