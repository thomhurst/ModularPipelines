using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "databases", "create")]
public record GcloudFirestoreDatabasesCreateOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliFlag("--delete-protection")]
    public bool? DeleteProtection { get; set; }

    [CliFlag("--enable-pitr")]
    public bool? EnablePitr { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}