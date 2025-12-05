using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "set-iam-policy")]
public record GcloudWorkstationsSetIamPolicyOptions(
[property: CliArgument] string Workstation,
[property: CliArgument] string Cluster,
[property: CliArgument] string Config,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;