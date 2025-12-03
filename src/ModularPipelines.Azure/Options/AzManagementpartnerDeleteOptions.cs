using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managementpartner", "delete")]
public record AzManagementpartnerDeleteOptions(
[property: CliOption("--partner-id")] string PartnerId
) : AzOptions;