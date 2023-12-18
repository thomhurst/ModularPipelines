using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "verified-partner", "show")]
public record AzEventgridPartnerVerifiedPartnerShowOptions(
[property: CommandSwitch("--verified-partner-name")] string VerifiedPartnerName
) : AzOptions;