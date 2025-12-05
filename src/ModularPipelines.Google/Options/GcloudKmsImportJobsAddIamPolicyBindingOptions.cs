using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-jobs", "add-iam-policy-binding")]
public record GcloudKmsImportJobsAddIamPolicyBindingOptions(
[property: CliArgument] string ImportJob,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;