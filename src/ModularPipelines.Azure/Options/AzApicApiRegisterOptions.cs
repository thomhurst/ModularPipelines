using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "api", "register")]
public record AzApicApiRegisterOptions(
[property: CliOption("--api-location")] string ApiLocation,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }
}