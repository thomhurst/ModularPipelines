using ModularPipelines.Helpers.Internal;
using TUnit.Assertions.Enums;

namespace ModularPipelines.TestHelpers;

public static class OptionsRenderingTestHelper
{
    public static IReadOnlyList<string> BuildArguments(object options)
    {
        var model = new CommandModelProvider().GetCommandModel(options.GetType());
        return new CommandArgumentBuilder().BuildArguments(model, options);
    }

    public static async Task AssertArguments(
        IEnumerable<string> actualArguments,
        IEnumerable<string> expectedArguments)
    {
        await Assert.That(actualArguments)
            .IsEquivalentTo(expectedArguments, CollectionOrdering.Matching);
    }
}
