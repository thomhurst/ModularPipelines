using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "get-ancestors-iam-policy")]
public record GcloudProjectsGetAncestorsIamPolicyOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;