using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "nv", "update")]
public record AzApimNvUpdateOptions(
[property: CliOption("--named-value-id")] string NamedValueId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliFlag("--secret")]
    public bool? Secret { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }
}