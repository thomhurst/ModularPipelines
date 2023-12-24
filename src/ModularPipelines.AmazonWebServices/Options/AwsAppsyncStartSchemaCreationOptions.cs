using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "start-schema-creation")]
public record AwsAppsyncStartSchemaCreationOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--definition")] string Definition
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}