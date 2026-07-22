using System.Collections.Specialized;
using ModularPipelines.Engine.Executors;

namespace ModularPipelines.UnitTests.Engine.Executors;

public class PipelineInitializerTests
{
    [Test]
    public async Task FormatEnvironmentVariables_DoesNotAddBlankLines()
    {
        var variables = new OrderedDictionary
        {
            ["FIRST"] = "one",
            ["SECOND"] = "two",
        };

        var result = PipelineInitializer.FormatEnvironmentVariables(variables);

        await Assert.That(result).IsEqualTo(
            $"FIRST: one{Environment.NewLine}SECOND: two{Environment.NewLine}");
    }
}
