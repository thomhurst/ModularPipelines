using System.Collections;
using ModularPipelines.Engine.Executors;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineInitializerTests
{
    [Test]
    public async Task FormatEnvironmentVariables_Does_Not_Add_Blank_Lines()
    {
        var variables = new Hashtable
        {
            ["FIRST"] = "one",
            ["SECOND"] = "two",
        };

        var output = PipelineInitializer.FormatEnvironmentVariables(variables);

        await Assert.That(output).DoesNotContain($"{Environment.NewLine}{Environment.NewLine}");
        await Assert.That(output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)).HasCount(2);
    }
}
