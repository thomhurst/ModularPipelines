using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "get-effective-iam-policy")]
public record GcloudAssetGetEffectiveIamPolicyOptions(
[property: CliOption("--names")] string[] Names,
[property: CliOption("--scope")] string Scope
) : GcloudOptions;