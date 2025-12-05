using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "get-iam-policy")]
public record GcloudProjectsGetIamPolicyOptions(
[property: CliArgument] string ProjectIdOrNumber
) : GcloudOptions;