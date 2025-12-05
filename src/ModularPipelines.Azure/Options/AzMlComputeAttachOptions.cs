using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "compute", "attach")]
public record AzMlComputeAttachOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--ssh-port")]
    public string? SshPort { get; set; }

    [CliOption("--ssh-private-key-file")]
    public string? SshPrivateKeyFile { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }
}