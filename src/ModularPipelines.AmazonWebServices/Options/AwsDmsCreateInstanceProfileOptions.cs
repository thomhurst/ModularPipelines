using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "create-instance-profile")]
public record AwsDmsCreateInstanceProfileOptions : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--instance-profile-name")]
    public string? InstanceProfileName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--subnet-group-identifier")]
    public string? SubnetGroupIdentifier { get; set; }

    [CommandSwitch("--vpc-security-groups")]
    public string[]? VpcSecurityGroups { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}