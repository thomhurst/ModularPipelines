using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "profile", "show")]
public record AzBillingProfileShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}