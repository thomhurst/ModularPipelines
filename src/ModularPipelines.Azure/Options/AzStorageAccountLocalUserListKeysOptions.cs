using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "local-user", "list-keys")]
public record AzStorageAccountLocalUserListKeysOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--has-shared-key")]
    public bool? HasSharedKey { get; set; }

    [BooleanCommandSwitch("--has-ssh-key")]
    public bool? HasSshKey { get; set; }

    [BooleanCommandSwitch("--has-ssh-password")]
    public bool? HasSshPassword { get; set; }

    [CommandSwitch("--home-directory")]
    public string? HomeDirectory { get; set; }

    [CommandSwitch("--permission-scope")]
    public string? PermissionScope { get; set; }

    [CommandSwitch("--ssh-authorized-key")]
    public string? SshAuthorizedKey { get; set; }
}