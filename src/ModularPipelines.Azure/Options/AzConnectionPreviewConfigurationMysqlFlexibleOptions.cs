using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "preview-configuration", "mysql-flexible")]
public record AzConnectionPreviewConfigurationMysqlFlexibleOptions : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--user-account")]
    public int? UserAccount { get; set; }
}