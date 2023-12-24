using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "register-container-instance")]
public record AwsEcsRegisterContainerInstanceOptions : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--instance-identity-document")]
    public string? InstanceIdentityDocument { get; set; }

    [CommandSwitch("--instance-identity-document-signature")]
    public string? InstanceIdentityDocumentSignature { get; set; }

    [CommandSwitch("--total-resources")]
    public string[]? TotalResources { get; set; }

    [CommandSwitch("--version-info")]
    public string? VersionInfo { get; set; }

    [CommandSwitch("--container-instance-arn")]
    public string? ContainerInstanceArn { get; set; }

    [CommandSwitch("--attributes")]
    public string[]? Attributes { get; set; }

    [CommandSwitch("--platform-devices")]
    public string[]? PlatformDevices { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}