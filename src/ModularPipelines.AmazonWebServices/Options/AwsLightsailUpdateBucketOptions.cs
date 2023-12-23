using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-bucket")]
public record AwsLightsailUpdateBucketOptions(
[property: CommandSwitch("--bucket-name")] string BucketName
) : AwsOptions
{
    [CommandSwitch("--access-rules")]
    public string? AccessRules { get; set; }

    [CommandSwitch("--versioning")]
    public string? Versioning { get; set; }

    [CommandSwitch("--readonly-access-accounts")]
    public string[]? ReadonlyAccessAccounts { get; set; }

    [CommandSwitch("--access-log-config")]
    public string? AccessLogConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}