using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "custom-jobs", "local-run")]
public record GcloudAiCustomJobsLocalRunOptions(
[property: PositionalArgument] string Args,
[property: CommandSwitch("--executor-image-uri")] string ExecutorImageUri
) : GcloudOptions
{
    [CommandSwitch("--extra-dirs")]
    public string[]? ExtraDirs { get; set; }

    [CommandSwitch("--extra-packages")]
    public string[]? ExtraPackages { get; set; }

    [BooleanCommandSwitch("--gpu")]
    public bool? Gpu { get; set; }

    [CommandSwitch("--local-package-path")]
    public string? LocalPackagePath { get; set; }

    [CommandSwitch("--output-image-uri")]
    public string? OutputImageUri { get; set; }

    [CommandSwitch("--requirements")]
    public string[]? Requirements { get; set; }

    [CommandSwitch("--service-account-key-file")]
    public string? ServiceAccountKeyFile { get; set; }

    [CommandSwitch("--python-module")]
    public string? PythonModule { get; set; }

    [CommandSwitch("--script")]
    public string? Script { get; set; }
}