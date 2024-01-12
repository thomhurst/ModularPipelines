using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-jobs", "add-iam-policy-binding")]
public record GcloudKmsImportJobsAddIamPolicyBindingOptions(
[property: PositionalArgument] string ImportJob,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;