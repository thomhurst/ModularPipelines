using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "clone-stack")]
public record AwsOpsworksCloneStackOptions(
[property: CommandSwitch("--source-stack-id")] string SourceStackId,
[property: CommandSwitch("--service-role-arn")] string ServiceRoleArn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--vpc-id")]
    public string? VpcId { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--default-instance-profile-arn")]
    public string? DefaultInstanceProfileArn { get; set; }

    [CommandSwitch("--default-os")]
    public string? DefaultOs { get; set; }

    [CommandSwitch("--hostname-theme")]
    public string? HostnameTheme { get; set; }

    [CommandSwitch("--default-availability-zone")]
    public string? DefaultAvailabilityZone { get; set; }

    [CommandSwitch("--default-subnet-id")]
    public string? DefaultSubnetId { get; set; }

    [CommandSwitch("--custom-json")]
    public string? CustomJson { get; set; }

    [CommandSwitch("--configuration-manager")]
    public string? ConfigurationManager { get; set; }

    [CommandSwitch("--chef-configuration")]
    public string? ChefConfiguration { get; set; }

    [CommandSwitch("--custom-cookbooks-source")]
    public string? CustomCookbooksSource { get; set; }

    [CommandSwitch("--default-ssh-key-name")]
    public string? DefaultSshKeyName { get; set; }

    [CommandSwitch("--clone-app-ids")]
    public string[]? CloneAppIds { get; set; }

    [CommandSwitch("--default-root-device-type")]
    public string? DefaultRootDeviceType { get; set; }

    [CommandSwitch("--agent-version")]
    public string? AgentVersion { get; set; }

    [CommandSwitch("--stack-region")]
    public string? StackRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}