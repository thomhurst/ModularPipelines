using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("braket", "get-job")]
public record AwsBraketGetJobOptions(
[property: CommandSwitch("--job-arn")] string JobArn
) : AwsOptions
{
    [CommandSwitch("--additional-attribute-names")]
    public string[]? AdditionalAttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}