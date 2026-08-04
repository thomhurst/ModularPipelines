using System.Text;
using ModularPipelines.Console;

namespace ModularPipelines.UnitTests.Console;

public class ModuleOutputExcerptBufferTests
{
    [Test]
    public async Task SeparatesStandardOutputAndStandardError()
    {
        var buffer = new ModuleOutputExcerptBuffer(1024);

        buffer.Append("normal output", ModuleOutputStream.StandardOutput);
        buffer.Append("error output", ModuleOutputStream.StandardError);

        var excerpt = buffer.CreateExcerpt();
        using (Assert.Multiple())
        {
            await Assert.That(excerpt!.StdoutTail).IsEqualTo("normal output" + Environment.NewLine);
            await Assert.That(excerpt.StderrTail).IsEqualTo("error output" + Environment.NewLine);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(0);
        }
    }

    [Test]
    public async Task AppliesOneUtf8TailLimitAcrossBothStreams()
    {
        const int maximumBytes = 10;
        var buffer = new ModuleOutputExcerptBuffer(maximumBytes);

        buffer.Append("old output", ModuleOutputStream.StandardOutput);
        buffer.Append("🙂new", ModuleOutputStream.StandardError);

        var excerpt = buffer.CreateExcerpt()!;
        var retainedBytes = Encoding.UTF8.GetByteCount(excerpt.StdoutTail ?? string.Empty)
                            + Encoding.UTF8.GetByteCount(excerpt.StderrTail ?? string.Empty);
        using (Assert.Multiple())
        {
            await Assert.That(retainedBytes).IsLessThanOrEqualTo(maximumBytes);
            await Assert.That(excerpt.StderrTail).EndsWith("🙂new" + Environment.NewLine);
            await Assert.That(excerpt.StdoutTail).DoesNotContain("old output");
            await Assert.That(excerpt.StdoutTail ?? string.Empty).DoesNotContain("�");
            await Assert.That(excerpt.StderrTail).DoesNotContain("�");
            await Assert.That(excerpt.TruncatedBytes).IsGreaterThan(0);
        }
    }

    [Test]
    public async Task RetainsValidUnicodeWhenTailStartsAtSurrogatePair()
    {
        var buffer = new ModuleOutputExcerptBuffer(6);

        buffer.Append("12345🙂", ModuleOutputStream.StandardOutput);

        var excerpt = buffer.CreateExcerpt()!;
        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo("🙂" + Environment.NewLine);
            await Assert.That(excerpt.StdoutTail).DoesNotContain("�");
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(5);
        }
    }
}
