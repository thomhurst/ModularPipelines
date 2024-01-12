using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "indexes", "fields", "describe")]
public record GcloudFirestoreIndexesFieldsDescribeOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string CollectionGroup,
[property: PositionalArgument] string Database
) : GcloudOptions;