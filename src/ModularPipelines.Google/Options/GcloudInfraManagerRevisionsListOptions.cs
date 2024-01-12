using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "revisions", "list")]
public record GcloudInfraManagerRevisionsListOptions(
[property: CommandSwitch("--deployment")] string Deployment,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;