using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "revisions", "list")]
public record GcloudInfraManagerRevisionsListOptions(
[property: CliOption("--deployment")] string Deployment,
[property: CliOption("--location")] string Location
) : GcloudOptions;