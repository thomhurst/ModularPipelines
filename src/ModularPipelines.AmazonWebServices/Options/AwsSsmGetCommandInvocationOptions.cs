using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-command-invocation")]
public record AwsSsmGetCommandInvocationOptions(
[property: CliOption("--command-id")] string CommandId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--plugin-name")]
    public string? PluginName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}