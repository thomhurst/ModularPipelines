using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "register-container-instance")]
public record AwsEcsRegisterContainerInstanceOptions : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--instance-identity-document")]
    public string? InstanceIdentityDocument { get; set; }

    [CliOption("--instance-identity-document-signature")]
    public string? InstanceIdentityDocumentSignature { get; set; }

    [CliOption("--total-resources")]
    public string[]? TotalResources { get; set; }

    [CliOption("--version-info")]
    public string? VersionInfo { get; set; }

    [CliOption("--container-instance-arn")]
    public string? ContainerInstanceArn { get; set; }

    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--platform-devices")]
    public string[]? PlatformDevices { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}