using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "databases", "describe")]
public record GcloudFirestoreDatabasesDescribeOptions : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}