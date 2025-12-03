using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "add-draft-app-version-resource-mappings")]
public record AwsResiliencehubAddDraftAppVersionResourceMappingsOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--resource-mappings")] string[] ResourceMappings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}