using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "get-iam-policy")]
public record GcloudArtifactsRepositoriesGetIamPolicyOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location
) : GcloudOptions;