using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "product", "test", "update")]
public record AzIotProductTestUpdateOptions(
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--at")]
    public string? At { get; set; }

    [CommandSwitch("--badge-type")]
    public string? BadgeType { get; set; }

    [CommandSwitch("--base-url")]
    public string? BaseUrl { get; set; }

    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--cf")]
    public string? Cf { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--ek")]
    public string? Ek { get; set; }

    [CommandSwitch("--models")]
    public string? Models { get; set; }
}

