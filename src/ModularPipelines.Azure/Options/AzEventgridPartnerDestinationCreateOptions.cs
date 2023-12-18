using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "destination", "create")]
public record AzEventgridPartnerDestinationCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--act-exp-date")]
    public string? ActExpDate { get; set; }

    [CommandSwitch("--ed-serv-cont")]
    public string? EdServCont { get; set; }

    [CommandSwitch("--endpoint-base-url")]
    public string? EndpointBaseUrl { get; set; }

    [CommandSwitch("--message-for-activation")]
    public string? MessageForActivation { get; set; }

    [CommandSwitch("--partner-registration-immutable-id")]
    public string? PartnerRegistrationImmutableId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}