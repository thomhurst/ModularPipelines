using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-security")]
public record AwsKafkaUpdateSecurityOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--client-authentication")]
    public string? ClientAuthentication { get; set; }

    [CommandSwitch("--encryption-info")]
    public string? EncryptionInfo { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}