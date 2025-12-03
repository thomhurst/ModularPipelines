using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "verified-partner", "show")]
public record AzEventgridPartnerVerifiedPartnerShowOptions(
[property: CliOption("--verified-partner-name")] string VerifiedPartnerName
) : AzOptions;