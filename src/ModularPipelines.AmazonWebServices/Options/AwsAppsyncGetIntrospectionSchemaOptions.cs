using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "get-introspection-schema")]
public record AwsAppsyncGetIntrospectionSchemaOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--format")] string Format
) : AwsOptions;