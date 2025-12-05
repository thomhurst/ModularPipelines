using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "import-statefile")]
public record GcloudInfraManagerDeploymentsImportStatefileOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location,
[property: CliOption("--lock-id")] string LockId
) : GcloudOptions;