using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "import")]
public record AzRedisImportOptions(
[property: CliOption("--files")] string Files
) : AzOptions
{
    [CliOption("--auth-method")]
    public string? AuthMethod { get; set; }

    [CliOption("--file-format")]
    public string? FileFormat { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}