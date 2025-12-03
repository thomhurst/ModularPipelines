using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "set-iam-policy")]
public record GcloudArtifactsRepositoriesSetIamPolicyOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;