using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "create-profile-job")]
public record AwsDatabrewCreateProfileJobOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--output-location")] string OutputLocation,
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

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--validation-configurations")]
    public string[]? ValidationConfigurations { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--job-sample")]
    public string? JobSample { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}