using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-job")]
public record AwsS3controlCreateJobOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--report")] string Report,
[property: CommandSwitch("--priority")] int Priority,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--manifest")]
    public string? Manifest { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--manifest-generator")]
    public string? ManifestGenerator { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}