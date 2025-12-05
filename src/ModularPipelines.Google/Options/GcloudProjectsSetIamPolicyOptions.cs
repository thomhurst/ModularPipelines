using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "set-iam-policy")]
public record GcloudProjectsSetIamPolicyOptions(
[property: CliArgument] string ProjectIdOrNumber,
[property: CliArgument] string PolicyFile
) : GcloudOptions;