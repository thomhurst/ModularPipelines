using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "databases", "describe")]
public record GcloudFirestoreDatabasesDescribeOptions : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }
}