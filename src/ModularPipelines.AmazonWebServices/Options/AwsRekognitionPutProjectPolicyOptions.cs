using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "put-project-policy")]
public record AwsRekognitionPutProjectPolicyOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CommandSwitch("--policy-revision-id")]
    public string? PolicyRevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}