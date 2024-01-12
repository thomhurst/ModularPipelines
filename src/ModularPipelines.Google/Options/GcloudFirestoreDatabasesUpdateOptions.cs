using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "databases", "update")]
public record GcloudFirestoreDatabasesUpdateOptions : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [BooleanCommandSwitch("--delete-protection")]
    public bool? DeleteProtection { get; set; }

    [BooleanCommandSwitch("--enable-pitr")]
    public bool? EnablePitr { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}