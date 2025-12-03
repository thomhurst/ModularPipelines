using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managementpartner", "create")]
public record AzManagementpartnerCreateOptions(
[property: CliOption("--partner-id")] string PartnerId
) : AzOptions;