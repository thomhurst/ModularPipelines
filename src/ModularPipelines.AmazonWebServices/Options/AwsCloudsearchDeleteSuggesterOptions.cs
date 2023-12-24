using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "delete-suggester")]
public record AwsCloudsearchDeleteSuggesterOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--suggester-name")] string SuggesterName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}