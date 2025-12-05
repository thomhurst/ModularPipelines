using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "list")]
public record GcloudComputeOsConfigPatchDeploymentsListOptions : GcloudOptions;