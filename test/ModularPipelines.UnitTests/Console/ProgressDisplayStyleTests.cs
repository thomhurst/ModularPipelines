using ModularPipelines.Console;

namespace ModularPipelines.UnitTests.Console;

public class ProgressDisplayStyleTests
{
    [Test]
    public async Task Format_Uses_Single_Cell_Ascii_Status_Markers()
    {
        var testCases = new[]
        {
            (ProgressTaskStatus.Pending, "[dim]. Build[/]"),
            (ProgressTaskStatus.Running, "[cyan]> Build[/]"),
            (ProgressTaskStatus.Succeeded, "[green]+ Build[/]"),
            (ProgressTaskStatus.Failed, "[red]! Build[/]"),
            (ProgressTaskStatus.Skipped, "[yellow]- Build[/]"),
        };

        foreach (var (status, expected) in testCases)
        {
            var result = ProgressDisplayStyle.FormatTask("Build", status);

            using (Assert.Multiple())
            {
                await Assert.That(result).IsEqualTo(expected);
                await Assert.That(result.All(character => character <= sbyte.MaxValue)).IsTrue();
            }
        }
    }

    [Test]
    public async Task Format_Indents_Submodules_Without_Changing_Marker()
    {
        var result = ProgressDisplayStyle.FormatTask(
            "Compile",
            ProgressTaskStatus.Running,
            isSubModule: true);

        await Assert.That(result).IsEqualTo("[cyan]  > Compile[/]");
    }

    [Test]
    public async Task Spinner_Uses_Only_Ascii_Frames()
    {
        await Assert.That(ProgressDisplayStyle.Spinner.Frames)
            .All(frame => frame.All(character => character <= sbyte.MaxValue));
    }

    [Test]
    public async Task Completed_Spinner_Is_Status_Neutral()
    {
        await Assert.That(ProgressDisplayStyle.CompletedSpinnerText)
            .IsEqualTo(" ");
    }
}
