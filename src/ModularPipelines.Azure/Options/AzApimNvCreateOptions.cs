using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "nv", "create")]
public record AzApimNvCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--named-value-id")] string NamedValueId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--secret")]
    public bool? Secret { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }
}