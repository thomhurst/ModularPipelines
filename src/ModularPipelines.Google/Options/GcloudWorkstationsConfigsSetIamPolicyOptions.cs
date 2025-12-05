using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "configs", "set-iam-policy")]
public record GcloudWorkstationsConfigsSetIamPolicyOptions(
[property: CliArgument] string Config,
[property: CliArgument] string Cluster,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;