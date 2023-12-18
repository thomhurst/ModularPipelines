using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "role-assignment", "show")]
public record AzBillingRoleAssignmentShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }
}