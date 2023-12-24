using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "create-studio")]
public record AwsEmrCreateStudioOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--auth-mode")] string AuthMode,
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds,
[property: CommandSwitch("--service-role")] string ServiceRole,
[property: CommandSwitch("--workspace-security-group-id")] string WorkspaceSecurityGroupId,
[property: CommandSwitch("--engine-security-group-id")] string EngineSecurityGroupId,
[property: CommandSwitch("--default-s3-location")] string DefaultS3Location
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--user-role")]
    public string? UserRole { get; set; }

    [CommandSwitch("--idp-auth-url")]
    public string? IdpAuthUrl { get; set; }

    [CommandSwitch("--idp-relay-state-parameter-name")]
    public string? IdpRelayStateParameterName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--idc-user-assignment")]
    public string? IdcUserAssignment { get; set; }

    [CommandSwitch("--idc-instance-arn")]
    public string? IdcInstanceArn { get; set; }

    [CommandSwitch("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}