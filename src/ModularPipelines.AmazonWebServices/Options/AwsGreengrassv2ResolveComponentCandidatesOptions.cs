using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "resolve-component-candidates")]
public record AwsGreengrassv2ResolveComponentCandidatesOptions : AwsOptions
{
    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--component-candidates")]
    public string[]? ComponentCandidates { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}