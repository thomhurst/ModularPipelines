using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class BuildSystemCommandWriterTests
{
    [Test]
    public async Task Writes_Long_Command_As_One_Unmodified_Line()
    {
        var output = new StringWriter();
        var writer = new BuildSystemCommandWriter(output);
        var command = $"::add-mask::{new string('x', 512)}";

        writer.WriteLine(command);

        await Assert.That(output.ToString()).IsEqualTo($"{command}{Environment.NewLine}");
    }

    [Test]
    public async Task Rejects_Multiple_Physical_Lines()
    {
        var writer = new BuildSystemCommandWriter(new StringWriter());

        await Assert.That(() => writer.WriteLine("::notice::first\nsecond"))
            .Throws<ArgumentException>();
    }
}
