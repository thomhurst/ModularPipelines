using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "ios", "models", "describe")]
public record GcloudFirebaseTestIosModelsDescribeOptions(
[property: CliArgument] string ModelId
) : GcloudOptions;