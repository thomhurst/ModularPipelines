using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "local-user", "update")]
public record AzStorageAccountLocalUserUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--has-shared-key")]
    public bool? HasSharedKey { get; set; }

    [CliFlag("--has-ssh-key")]
    public bool? HasSshKey { get; set; }

    [CliFlag("--has-ssh-password")]
    public bool? HasSshPassword { get; set; }

    [CliOption("--home-directory")]
    public string? HomeDirectory { get; set; }

    [CliOption("--permission-scope")]
    public string? PermissionScope { get; set; }

    [CliOption("--ssh-authorized-key")]
    public string? SshAuthorizedKey { get; set; }
}