using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connection", "validate")]
public record AzConnectionValidateOptions : AzOptions
{
    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}