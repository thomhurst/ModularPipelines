using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "verified-partner", "show")]
public record AzEventgridPartnerVerifiedPartnerShowOptions(
[property: CommandSwitch("--verified-partner-name")] string VerifiedPartnerName
) : AzOptions
{
}