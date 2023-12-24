using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "list-data-lakes")]
public record AwsSecuritylakeListDataLakesOptions : AwsOptions
{
    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}