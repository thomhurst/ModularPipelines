using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "or-policy", "update")]
public record AzStorageAccountOrPolicyUpdateOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--destination-account")]
    public int? DestinationAccount { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--policy-id")]
    public string? PolicyId { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source-account")]
    public int? SourceAccount { get; set; }
}