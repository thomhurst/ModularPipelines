using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managementpartner", "update")]
public record AzManagementpartnerUpdateOptions(
[property: CliOption("--partner-id")] string PartnerId
) : AzOptions;