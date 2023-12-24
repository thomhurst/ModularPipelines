using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "define-suggester")]
public record AwsCloudsearchDefineSuggesterOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--suggester")] string Suggester
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}