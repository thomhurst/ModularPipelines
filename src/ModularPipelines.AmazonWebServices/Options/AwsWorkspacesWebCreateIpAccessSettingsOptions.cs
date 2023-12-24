using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "create-ip-access-settings")]
public record AwsWorkspacesWebCreateIpAccessSettingsOptions(
[property: CommandSwitch("--ip-rules")] string[] IpRules
) : AwsOptions
{
    [CommandSwitch("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}