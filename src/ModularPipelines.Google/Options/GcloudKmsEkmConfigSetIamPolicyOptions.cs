using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-config", "set-iam-policy")]
public record GcloudKmsEkmConfigSetIamPolicyOptions(
[property: CliArgument] string PolicyFile,
[property: CliOption("--location")] string Location
) : GcloudOptions;