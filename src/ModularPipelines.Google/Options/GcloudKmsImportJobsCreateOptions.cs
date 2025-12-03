using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-jobs", "create")]
public record GcloudKmsImportJobsCreateOptions(
[property: CliArgument] string ImportJob,
[property: CliOption("--import-method")] string ImportMethod,
[property: CliOption("--protection-level")] string ProtectionLevel
) : GcloudOptions
{
    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}