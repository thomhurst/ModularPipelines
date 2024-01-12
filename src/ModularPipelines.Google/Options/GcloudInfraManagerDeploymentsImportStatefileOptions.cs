using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "import-statefile")]
public record GcloudInfraManagerDeploymentsImportStatefileOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--lock-id")] string LockId
) : GcloudOptions;