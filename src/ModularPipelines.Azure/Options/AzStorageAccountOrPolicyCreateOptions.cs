using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "or-policy", "create")]
public record AzStorageAccountOrPolicyCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--dcont")]
    public string? Dcont { get; set; }

    [CommandSwitch("--destination-account")]
    public int? DestinationAccount { get; set; }

    [CommandSwitch("--min-creation-time")]
    public string? MinCreationTime { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--policy-id")]
    public string? PolicyId { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-id")]
    public string? RuleId { get; set; }

    [CommandSwitch("--scont")]
    public string? Scont { get; set; }

    [CommandSwitch("--source-account")]
    public int? SourceAccount { get; set; }
}

