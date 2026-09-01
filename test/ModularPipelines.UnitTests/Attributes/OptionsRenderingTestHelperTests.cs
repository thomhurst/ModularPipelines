using TUnit.Assertions.Exceptions;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests.Attributes;

public class OptionsRenderingTestHelperTests
{
    [Test]
    public async Task Argument_Comparison_Rejects_Reordered_Tokens()
    {
        static Task CompareReorderedArguments() =>
            AssertArguments(["--option", "value"], ["value", "--option"]);

        await Assert.That(CompareReorderedArguments)
            .Throws<AssertionException>();
    }
}
