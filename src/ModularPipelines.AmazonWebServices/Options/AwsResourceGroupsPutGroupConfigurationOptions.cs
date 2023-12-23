using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "put-group-configuration")]
public record AwsResourceGroupsPutGroupConfigurationOptions : AwsOptions
{
    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--configuration")]
    public string[]? Configuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}