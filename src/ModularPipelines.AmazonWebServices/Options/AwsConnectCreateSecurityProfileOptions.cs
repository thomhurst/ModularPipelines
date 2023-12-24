using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-security-profile")]
public record AwsConnectCreateSecurityProfileOptions(
[property: CommandSwitch("--security-profile-name")] string SecurityProfileName,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--allowed-access-control-tags")]
    public IEnumerable<KeyValue>? AllowedAccessControlTags { get; set; }

    [CommandSwitch("--tag-restricted-resources")]
    public string[]? TagRestrictedResources { get; set; }

    [CommandSwitch("--applications")]
    public string[]? Applications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}