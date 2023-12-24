using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "get-plugin")]
public record AwsQbusinessGetPluginOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--plugin-id")] string PluginId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}