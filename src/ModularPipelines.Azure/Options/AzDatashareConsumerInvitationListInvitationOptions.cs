using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "consumer-invitation", "list-invitation")]
public record AzDatashareConsumerInvitationListInvitationOptions : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}