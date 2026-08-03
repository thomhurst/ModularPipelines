using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.TestHelpers;

public static class OptionsRenderingTestHelper
{
    public static IReadOnlyList<string> BuildArguments(object options)
    {
        var model = new CommandModelProvider().GetCommandModel(options.GetType());
        return new CommandArgumentBuilder().BuildArguments(model, options);
    }
}
