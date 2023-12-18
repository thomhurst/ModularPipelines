using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "account", "list")]
public record AzBillingAccountListOptions : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}