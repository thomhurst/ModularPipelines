using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-jobs", "set-iam-policy")]
public record GcloudKmsImportJobsSetIamPolicyOptions(
[property: CliArgument] string ImportJob,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;