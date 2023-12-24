using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "update-recipe-job")]
public record AwsDatabrewUpdateRecipeJobOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CommandSwitch("--encryption-mode")]
    public string? EncryptionMode { get; set; }

    [CommandSwitch("--log-subscription")]
    public string? LogSubscription { get; set; }

    [CommandSwitch("--max-capacity")]
    public int? MaxCapacity { get; set; }

    [CommandSwitch("--max-retries")]
    public int? MaxRetries { get; set; }

    [CommandSwitch("--outputs")]
    public string[]? Outputs { get; set; }

    [CommandSwitch("--data-catalog-outputs")]
    public string[]? DataCatalogOutputs { get; set; }

    [CommandSwitch("--database-outputs")]
    public string[]? DatabaseOutputs { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}