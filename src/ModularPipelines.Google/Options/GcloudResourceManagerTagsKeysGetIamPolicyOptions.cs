using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "keys", "get-iam-policy")]
public record GcloudResourceManagerTagsKeysGetIamPolicyOptions(
[property: CliArgument] string ResourceName
) : GcloudOptions;