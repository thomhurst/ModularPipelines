using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "list-domain-names")]
public record AwsOpensearchListDomainNamesOptions : AwsOptions
{
    [CommandSwitch("--engine-type")]
    public string? EngineType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}