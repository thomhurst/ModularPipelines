using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearchdomain", "suggest")]
public record AwsCloudsearchdomainSuggestOptions(
[property: CommandSwitch("--suggester")] string Suggester,
[property: CommandSwitch("--suggest-query")] string SuggestQuery
) : AwsOptions
{
    [CommandSwitch("--size")]
    public long? Size { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}