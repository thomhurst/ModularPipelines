using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "update-stack")]
public record AwsOpsworksUpdateStackOptions(
[property: CommandSwitch("--stack-id")] string StackId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

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

    [CommandSwitch("--default-root-device-type")]
    public string? DefaultRootDeviceType { get; set; }

    [CommandSwitch("--agent-version")]
    public string? AgentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}