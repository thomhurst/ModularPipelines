using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "get-managed-endpoint-session-credentials")]
public record AwsEmrContainersGetManagedEndpointSessionCredentialsOptions(
[property: CommandSwitch("--endpoint-identifier")] string EndpointIdentifier,
[property: CommandSwitch("--virtual-cluster-identifier")] string VirtualClusterIdentifier,
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--credential-type")] string CredentialType
) : AwsOptions
{
    [CommandSwitch("--duration-in-seconds")]
    public int? DurationInSeconds { get; set; }

    [CommandSwitch("--log-context")]
    public string? LogContext { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}