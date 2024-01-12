using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "revisions", "export-statefile")]
public record GcloudInfraManagerRevisionsExportStatefileOptions(
[property: PositionalArgument] string Revision,
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions;