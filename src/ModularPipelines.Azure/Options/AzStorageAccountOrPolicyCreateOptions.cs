using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "or-policy", "create")]
public record AzStorageAccountOrPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--dcont")]
    public string? Dcont { get; set; }

    [CliOption("--destination-account")]
    public int? DestinationAccount { get; set; }

    [CliOption("--min-creation-time")]
    public string? MinCreationTime { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--policy-id")]
    public string? PolicyId { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-id")]
    public string? RuleId { get; set; }

    [CliOption("--scont")]
    public string? Scont { get; set; }

    [CliOption("--source-account")]
    public int? SourceAccount { get; set; }
}