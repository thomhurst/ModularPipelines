using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "create-studio")]
public record AwsEmrCreateStudioOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--auth-mode")] string AuthMode,
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--service-role")] string ServiceRole,
[property: CliOption("--workspace-security-group-id")] string WorkspaceSecurityGroupId,
[property: CliOption("--engine-security-group-id")] string EngineSecurityGroupId,
[property: CliOption("--default-s3-location")] string DefaultS3Location
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--user-role")]
    public string? UserRole { get; set; }

    [CliOption("--idp-auth-url")]
    public string? IdpAuthUrl { get; set; }

    [CliOption("--idp-relay-state-parameter-name")]
    public string? IdpRelayStateParameterName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--idc-user-assignment")]
    public string? IdcUserAssignment { get; set; }

    [CliOption("--idc-instance-arn")]
    public string? IdcInstanceArn { get; set; }

    [CliOption("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}