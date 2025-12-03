using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-verified-access-instance-logging-configuration")]
public record AwsEc2ModifyVerifiedAccessInstanceLoggingConfigurationOptions(
[property: CliOption("--verified-access-instance-id")] string VerifiedAccessInstanceId,
[property: CliOption("--access-logs")] string AccessLogs
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}