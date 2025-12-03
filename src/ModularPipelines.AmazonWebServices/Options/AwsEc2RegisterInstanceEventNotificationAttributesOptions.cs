using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "register-instance-event-notification-attributes")]
public record AwsEc2RegisterInstanceEventNotificationAttributesOptions(
[property: CliOption("--instance-tag-attribute")] string InstanceTagAttribute
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}