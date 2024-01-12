using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "ios", "models", "describe")]
public record GcloudFirebaseTestIosModelsDescribeOptions(
[property: PositionalArgument] string ModelId
) : GcloudOptions;