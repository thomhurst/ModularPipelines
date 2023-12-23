using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-security-profile")]
public record AwsConnectUpdateSecurityProfileOptions(
[property: CommandSwitch("--security-profile-id")] string SecurityProfileId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--allowed-access-control-tags")]
    public IEnumerable<KeyValue>? AllowedAccessControlTags { get; set; }

    [CommandSwitch("--tag-restricted-resources")]
    public string[]? TagRestrictedResources { get; set; }

    [CommandSwitch("--applications")]
    public string[]? Applications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}