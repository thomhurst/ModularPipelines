using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "start-thing-registration-task")]
public record AwsIotStartThingRegistrationTaskOptions(
[property: CommandSwitch("--template-body")] string TemplateBody,
[property: CommandSwitch("--input-file-bucket")] string InputFileBucket,
[property: CommandSwitch("--input-file-key")] string InputFileKey,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}