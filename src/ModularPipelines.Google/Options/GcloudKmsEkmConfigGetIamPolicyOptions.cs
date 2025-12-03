using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-config", "get-iam-policy")]
public record GcloudKmsEkmConfigGetIamPolicyOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;