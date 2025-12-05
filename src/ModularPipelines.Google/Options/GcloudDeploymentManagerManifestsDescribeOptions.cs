using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "manifests", "describe")]
public record GcloudDeploymentManagerManifestsDescribeOptions(
[property: CliArgument] string Manifest,
[property: CliOption("--deployment")] string Deployment
) : GcloudOptions;