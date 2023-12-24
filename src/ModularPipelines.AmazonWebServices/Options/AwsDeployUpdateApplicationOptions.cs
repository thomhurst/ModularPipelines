using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "update-application")]
public record AwsDeployUpdateApplicationOptions : AwsOptions
{
    [CommandSwitch("--application-name")]
    public string? ApplicationName { get; set; }

    [CommandSwitch("--new-application-name")]
    public string? NewApplicationName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}