using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "destination", "create")]
public record AzEventgridPartnerDestinationCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--act-exp-date")]
    public string? ActExpDate { get; set; }

    [CliOption("--ed-serv-cont")]
    public string? EdServCont { get; set; }

    [CliOption("--endpoint-base-url")]
    public string? EndpointBaseUrl { get; set; }

    [CliOption("--message-for-activation")]
    public string? MessageForActivation { get; set; }

    [CliOption("--partner-registration-immutable-id")]
    public string? PartnerRegistrationImmutableId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}