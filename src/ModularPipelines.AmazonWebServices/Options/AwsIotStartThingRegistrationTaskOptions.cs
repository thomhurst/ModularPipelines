using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "start-thing-registration-task")]
public record AwsIotStartThingRegistrationTaskOptions(
[property: CliOption("--template-body")] string TemplateBody,
[property: CliOption("--input-file-bucket")] string InputFileBucket,
[property: CliOption("--input-file-key")] string InputFileKey,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}