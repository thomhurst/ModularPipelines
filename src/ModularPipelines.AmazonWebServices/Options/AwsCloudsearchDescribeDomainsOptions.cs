using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "describe-domains")]
public record AwsCloudsearchDescribeDomainsOptions : AwsOptions
{
    [CommandSwitch("--domain-names")]
    public string[]? DomainNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}