using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "consumer-invitation", "list-invitation")]
public record AzDatashareConsumerInvitationListInvitationOptions : AzOptions
{
    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}