using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "get-compatible-versions")]
public record AwsOpensearchGetCompatibleVersionsOptions : AwsOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}