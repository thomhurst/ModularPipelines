using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-containers", "get-managed-endpoint-session-credentials")]
public record AwsEmrContainersGetManagedEndpointSessionCredentialsOptions(
[property: CliOption("--endpoint-identifier")] string EndpointIdentifier,
[property: CliOption("--virtual-cluster-identifier")] string VirtualClusterIdentifier,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--credential-type")] string CredentialType
) : AwsOptions
{
    [CliOption("--duration-in-seconds")]
    public int? DurationInSeconds { get; set; }

    [CliOption("--log-context")]
    public string? LogContext { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}