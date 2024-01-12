using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-jobs", "create")]
public record GcloudKmsImportJobsCreateOptions(
[property: PositionalArgument] string ImportJob,
[property: CommandSwitch("--import-method")] string ImportMethod,
[property: CommandSwitch("--protection-level")] string ProtectionLevel
) : GcloudOptions
{
    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}