using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-verified-access-instance-logging-configuration")]
public record AwsEc2ModifyVerifiedAccessInstanceLoggingConfigurationOptions(
[property: CommandSwitch("--verified-access-instance-id")] string VerifiedAccessInstanceId,
[property: CommandSwitch("--access-logs")] string AccessLogs
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}