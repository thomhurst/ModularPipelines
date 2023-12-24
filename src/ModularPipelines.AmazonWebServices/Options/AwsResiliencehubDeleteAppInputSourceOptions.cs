using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "delete-app-input-source")]
public record AwsResiliencehubDeleteAppInputSourceOptions(
[property: CommandSwitch("--app-arn")] string AppArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--eks-source-cluster-namespace")]
    public string? EksSourceClusterNamespace { get; set; }

    [CommandSwitch("--source-arn")]
    public string? SourceArn { get; set; }

    [CommandSwitch("--terraform-source")]
    public string? TerraformSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}