using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "describe-addon-versions")]
public record AwsEksDescribeAddonVersionsOptions : AwsOptions
{
    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--addon-name")]
    public string? AddonName { get; set; }

    [CommandSwitch("--types")]
    public string[]? Types { get; set; }

    [CommandSwitch("--publishers")]
    public string[]? Publishers { get; set; }

    [CommandSwitch("--owners")]
    public string[]? Owners { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}