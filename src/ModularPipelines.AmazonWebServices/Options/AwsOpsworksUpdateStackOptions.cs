using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "update-stack")]
public record AwsOpsworksUpdateStackOptions(
[property: CliOption("--stack-id")] string StackId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CliOption("--default-instance-profile-arn")]
    public string? DefaultInstanceProfileArn { get; set; }

    [CliOption("--default-os")]
    public string? DefaultOs { get; set; }

    [CliOption("--hostname-theme")]
    public string? HostnameTheme { get; set; }

    [CliOption("--default-availability-zone")]
    public string? DefaultAvailabilityZone { get; set; }

    [CliOption("--default-subnet-id")]
    public string? DefaultSubnetId { get; set; }

    [CliOption("--custom-json")]
    public string? CustomJson { get; set; }

    [CliOption("--configuration-manager")]
    public string? ConfigurationManager { get; set; }

    [CliOption("--chef-configuration")]
    public string? ChefConfiguration { get; set; }

    [CliOption("--custom-cookbooks-source")]
    public string? CustomCookbooksSource { get; set; }

    [CliOption("--default-ssh-key-name")]
    public string? DefaultSshKeyName { get; set; }

    [CliOption("--default-root-device-type")]
    public string? DefaultRootDeviceType { get; set; }

    [CliOption("--agent-version")]
    public string? AgentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}