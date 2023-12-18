using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "enrollment-account", "list")]
public record AzBillingEnrollmentAccountListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}