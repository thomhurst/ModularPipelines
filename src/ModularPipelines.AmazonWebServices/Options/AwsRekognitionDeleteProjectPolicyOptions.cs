using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "delete-project-policy")]
public record AwsRekognitionDeleteProjectPolicyOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn,
[property: CommandSwitch("--policy-name")] string PolicyName
) : AwsOptions
{
    [CommandSwitch("--policy-revision-id")]
    public string? PolicyRevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}