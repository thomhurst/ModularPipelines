using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-jobs", "remove-iam-policy-binding")]
public record GcloudKmsImportJobsRemoveIamPolicyBindingOptions(
[property: CliArgument] string ImportJob,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;