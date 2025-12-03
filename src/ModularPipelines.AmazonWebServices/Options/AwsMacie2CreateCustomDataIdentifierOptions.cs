using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "create-custom-data-identifier")]
public record AwsMacie2CreateCustomDataIdentifierOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--regex")] string Regex
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ignore-words")]
    public string[]? IgnoreWords { get; set; }

    [CliOption("--keywords")]
    public string[]? Keywords { get; set; }

    [CliOption("--maximum-match-distance")]
    public int? MaximumMatchDistance { get; set; }

    [CliOption("--severity-levels")]
    public string[]? SeverityLevels { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}