using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "user", "update")]
public record AzVmUserUpdateOptions(
[property: CliOption("--username")] string Username
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}