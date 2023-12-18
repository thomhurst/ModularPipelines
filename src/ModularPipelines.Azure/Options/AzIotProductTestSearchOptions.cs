using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "product", "test", "search")]
public record AzIotProductTestSearchOptions(
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--base-url")]
    public string? BaseUrl { get; set; }

    [CommandSwitch("--certificate-name")]
    public string? CertificateName { get; set; }

    [CommandSwitch("--product-id")]
    public string? ProductId { get; set; }

    [CommandSwitch("--registration-id")]
    public string? RegistrationId { get; set; }
}

