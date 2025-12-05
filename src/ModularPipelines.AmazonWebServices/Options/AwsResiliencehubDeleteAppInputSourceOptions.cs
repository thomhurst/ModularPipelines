using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "delete-app-input-source")]
public record AwsResiliencehubDeleteAppInputSourceOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--eks-source-cluster-namespace")]
    public string? EksSourceClusterNamespace { get; set; }

    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--terraform-source")]
    public string? TerraformSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}