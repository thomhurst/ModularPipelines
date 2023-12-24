using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "add-draft-app-version-resource-mappings")]
public record AwsResiliencehubAddDraftAppVersionResourceMappingsOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--resource-mappings")] string[] ResourceMappings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}