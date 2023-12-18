using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "or-policy", "rule", "add")]
public record AzStorageAccountOrPolicyRuleAddOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--destination-container")] string DestinationContainer,
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--source-container")] string SourceContainer
) : AzOptions
{
    [CommandSwitch("--min-creation-time")]
    public string? MinCreationTime { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}