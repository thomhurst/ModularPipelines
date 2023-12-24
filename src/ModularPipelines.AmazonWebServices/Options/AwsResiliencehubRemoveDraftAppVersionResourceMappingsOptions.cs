using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "remove-draft-app-version-resource-mappings")]
public record AwsResiliencehubRemoveDraftAppVersionResourceMappingsOptions(
[property: CommandSwitch("--app-arn")] string AppArn
) : AwsOptions
{
    [CommandSwitch("--app-registry-app-names")]
    public string[]? AppRegistryAppNames { get; set; }

    [CommandSwitch("--eks-source-names")]
    public string[]? EksSourceNames { get; set; }

    [CommandSwitch("--logical-stack-names")]
    public string[]? LogicalStackNames { get; set; }

    [CommandSwitch("--resource-group-names")]
    public string[]? ResourceGroupNames { get; set; }

    [CommandSwitch("--resource-names")]
    public string[]? ResourceNames { get; set; }

    [CommandSwitch("--terraform-source-names")]
    public string[]? TerraformSourceNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}