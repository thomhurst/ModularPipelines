using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-jobs", "get-iam-policy")]
public record GcloudKmsImportJobsGetIamPolicyOptions(
[property: PositionalArgument] string ImportJob,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location
) : GcloudOptions;