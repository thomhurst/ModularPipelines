using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "remove-attributes")]
public record AwsPinpointRemoveAttributesOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--attribute-type")] string AttributeType,
[property: CommandSwitch("--update-attributes-request")] string UpdateAttributesRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}