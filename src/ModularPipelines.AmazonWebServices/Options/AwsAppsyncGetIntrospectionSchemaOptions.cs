using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "get-introspection-schema")]
public record AwsAppsyncGetIntrospectionSchemaOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--format")] string Format
) : AwsOptions;