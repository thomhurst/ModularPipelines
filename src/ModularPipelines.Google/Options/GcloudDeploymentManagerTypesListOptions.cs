using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "types", "list")]
public record GcloudDeploymentManagerTypesListOptions : GcloudOptions;