using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-config", "get-iam-policy")]
public record GcloudKmsEkmConfigGetIamPolicyOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;