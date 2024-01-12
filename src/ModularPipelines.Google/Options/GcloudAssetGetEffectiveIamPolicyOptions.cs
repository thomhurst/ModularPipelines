using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "get-effective-iam-policy")]
public record GcloudAssetGetEffectiveIamPolicyOptions(
[property: CommandSwitch("--names")] string[] Names,
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions;