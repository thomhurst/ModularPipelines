using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "delete-resources-by-external-id")]
public record AwsDeployDeleteResourcesByExternalIdOptions : AwsOptions
{
    [CommandSwitch("--external-id")]
    public string? ExternalId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}