using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "databases", "update")]
public record GcloudFirestoreDatabasesUpdateOptions : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliFlag("--delete-protection")]
    public bool? DeleteProtection { get; set; }

    [CliFlag("--enable-pitr")]
    public bool? EnablePitr { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}