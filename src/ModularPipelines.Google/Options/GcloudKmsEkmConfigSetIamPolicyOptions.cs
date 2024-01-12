using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-config", "set-iam-policy")]
public record GcloudKmsEkmConfigSetIamPolicyOptions(
[property: PositionalArgument] string PolicyFile,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;