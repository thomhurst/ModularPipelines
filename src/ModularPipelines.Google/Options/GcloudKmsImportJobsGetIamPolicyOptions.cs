using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-jobs", "get-iam-policy")]
public record GcloudKmsImportJobsGetIamPolicyOptions(
[property: CliArgument] string ImportJob,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location
) : GcloudOptions;