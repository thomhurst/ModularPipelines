using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "revisions", "export-statefile")]
public record GcloudInfraManagerRevisionsExportStatefileOptions(
[property: CliArgument] string Revision,
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions;