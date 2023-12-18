using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "enrollment-account", "show")]
public record AzBillingEnrollmentAccountShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}