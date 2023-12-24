using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "delete-plugin")]
public record AwsQbusinessDeletePluginOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--plugin-id")] string PluginId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}