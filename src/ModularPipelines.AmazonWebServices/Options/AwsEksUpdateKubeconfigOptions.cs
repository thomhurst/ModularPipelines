using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "update-kubeconfig")]
public record AwsEksUpdateKubeconfigOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--user-alias")]
    public string? UserAlias { get; set; }
}