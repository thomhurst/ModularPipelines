using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "create-recipe-job")]
public record AwsDatabrewCreateRecipeJobOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--dataset-name")]
    public string? DatasetName { get; set; }

    [CliOption("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CliOption("--encryption-mode")]
    public string? EncryptionMode { get; set; }

    [CliOption("--log-subscription")]
    public string? LogSubscription { get; set; }

    [CliOption("--max-capacity")]
    public int? MaxCapacity { get; set; }

    [CliOption("--max-retries")]
    public int? MaxRetries { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--data-catalog-outputs")]
    public string[]? DataCatalogOutputs { get; set; }

    [CliOption("--database-outputs")]
    public string[]? DatabaseOutputs { get; set; }

    [CliOption("--project-name")]
    public string? ProjectName { get; set; }

    [CliOption("--recipe-reference")]
    public string? RecipeReference { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}