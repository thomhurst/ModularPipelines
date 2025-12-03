using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "remove-draft-app-version-resource-mappings")]
public record AwsResiliencehubRemoveDraftAppVersionResourceMappingsOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--app-registry-app-names")]
    public string[]? AppRegistryAppNames { get; set; }

    [CliOption("--eks-source-names")]
    public string[]? EksSourceNames { get; set; }

    [CliOption("--logical-stack-names")]
    public string[]? LogicalStackNames { get; set; }

    [CliOption("--resource-group-names")]
    public string[]? ResourceGroupNames { get; set; }

    [CliOption("--resource-names")]
    public string[]? ResourceNames { get; set; }

    [CliOption("--terraform-source-names")]
    public string[]? TerraformSourceNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}