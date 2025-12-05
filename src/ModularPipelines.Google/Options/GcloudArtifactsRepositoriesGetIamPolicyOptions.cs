using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "get-iam-policy")]
public record GcloudArtifactsRepositoriesGetIamPolicyOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location
) : GcloudOptions;