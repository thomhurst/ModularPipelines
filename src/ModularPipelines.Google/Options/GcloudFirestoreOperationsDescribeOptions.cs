using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "operations", "describe")]
public record GcloudFirestoreOperationsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}