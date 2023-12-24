using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "create-custom-data-identifier")]
public record AwsMacie2CreateCustomDataIdentifierOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--regex")] string Regex
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ignore-words")]
    public string[]? IgnoreWords { get; set; }

    [CommandSwitch("--keywords")]
    public string[]? Keywords { get; set; }

    [CommandSwitch("--maximum-match-distance")]
    public int? MaximumMatchDistance { get; set; }

    [CommandSwitch("--severity-levels")]
    public string[]? SeverityLevels { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}