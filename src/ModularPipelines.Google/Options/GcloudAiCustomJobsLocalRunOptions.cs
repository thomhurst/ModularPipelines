using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "custom-jobs", "local-run")]
public record GcloudAiCustomJobsLocalRunOptions(
[property: CliArgument] string Args,
[property: CliOption("--executor-image-uri")] string ExecutorImageUri
) : GcloudOptions
{
    [CliOption("--extra-dirs")]
    public string[]? ExtraDirs { get; set; }

    [CliOption("--extra-packages")]
    public string[]? ExtraPackages { get; set; }

    [CliFlag("--gpu")]
    public bool? Gpu { get; set; }

    [CliOption("--local-package-path")]
    public string? LocalPackagePath { get; set; }

    [CliOption("--output-image-uri")]
    public string? OutputImageUri { get; set; }

    [CliOption("--requirements")]
    public string[]? Requirements { get; set; }

    [CliOption("--service-account-key-file")]
    public string? ServiceAccountKeyFile { get; set; }

    [CliOption("--python-module")]
    public string? PythonModule { get; set; }

    [CliOption("--script")]
    public string? Script { get; set; }
}