using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "indexes", "fields", "describe")]
public record GcloudFirestoreIndexesFieldsDescribeOptions(
[property: CliArgument] string Field,
[property: CliArgument] string CollectionGroup,
[property: CliArgument] string Database
) : GcloudOptions;