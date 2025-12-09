using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("image", "builder", "output", "versioning", "set")]
public record AzImageBuilderOutputVersioningSetOptions(
[property: CliOption("--output-name")] string OutputName,
[property: CliOption("--scheme")] string Scheme
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--major")]
    public string? Major { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}