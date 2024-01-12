using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-jobs", "set-iam-policy")]
public record GcloudKmsImportJobsSetIamPolicyOptions(
[property: PositionalArgument] string ImportJob,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;