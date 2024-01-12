using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "search")]
public record GcloudIdentityGroupsSearchOptions(
[property: CommandSwitch("--labels")] string Labels,
[property: CommandSwitch("--customer")] string Customer,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--view")]
    public string? View { get; set; }
}