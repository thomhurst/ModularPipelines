using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container", "create")]
public record AzStorageContainerCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--default-encryption-scope")]
    public string? DefaultEncryptionScope { get; set; }

    [BooleanCommandSwitch("--fail-on-exist")]
    public bool? FailOnExist { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [BooleanCommandSwitch("--prevent-encryption-scope-override")]
    public bool? PreventEncryptionScopeOverride { get; set; }

    [CommandSwitch("--public-access")]
    public string? PublicAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}

