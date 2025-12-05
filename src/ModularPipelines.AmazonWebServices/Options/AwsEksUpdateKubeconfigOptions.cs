using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "update-kubeconfig")]
public record AwsEksUpdateKubeconfigOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--verbose")]
    public bool? Verbose { get; set; }

    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--user-alias")]
    public string? UserAlias { get; set; }
}