using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "api-token", "create")]
public record AzIotCentralApiTokenCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--tkid")] string Tkid
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--org-id")]
    public string? OrgId { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}