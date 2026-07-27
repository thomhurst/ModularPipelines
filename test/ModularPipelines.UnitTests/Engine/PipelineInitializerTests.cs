using System.Collections;
using ModularPipelines.Engine.Executors;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineInitializerTests
{
    [Test]
    public async Task EnvironmentVariablesTable_Does_Not_Add_Blank_Rows()
    {
        var variables = new Hashtable
        {
            ["FIRST"] = "one",
            ["SECOND"] = "two",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);

        await Assert.That(table.Rows).Count().IsEqualTo(2);
    }
}
