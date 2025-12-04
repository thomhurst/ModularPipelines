using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "namespace", "create", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceCreateEventgridExtensionOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-registration-id")] string PartnerRegistrationId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}