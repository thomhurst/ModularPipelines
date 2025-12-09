using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-instance-profile")]
public record AwsDmsCreateInstanceProfileOptions : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--instance-profile-name")]
    public string? InstanceProfileName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--subnet-group-identifier")]
    public string? SubnetGroupIdentifier { get; set; }

    [CliOption("--vpc-security-groups")]
    public string[]? VpcSecurityGroups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}