using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "sign-url")]
public record GcloudStorageSignUrlOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CliOption("--http-verb")]
    public string? HttpVerb { get; set; }

    [CliOption("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CliOption("--private-key-password")]
    public string? PrivateKeyPassword { get; set; }

    [CliOption("--query-params")]
    public IEnumerable<KeyValue>? QueryParams { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}