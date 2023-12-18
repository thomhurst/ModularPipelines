using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "logout")]
public record AzDevopsLogoutOptions : AzOptions
{
    [CommandSwitch("--org")]
    public string? Org { get; set; }
}