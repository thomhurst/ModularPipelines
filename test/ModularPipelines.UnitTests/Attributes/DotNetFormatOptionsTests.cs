using ModularPipelines.DotNet.Options;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class DotNetFormatOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    private List<string> BuildArguments(object optionsObject)
    {
        var model = _modelProvider.GetCommandModel(optionsObject.GetType());
        return _argumentBuilder.BuildArguments(model, optionsObject);
    }

    [Test]
    public async Task ExcludeDiagnostics_Passes_Each_Id_Separately()
    {
        var options = new DotNetFormatOptions
        {
            ExcludeDiagnostics = ["CS0246", "CS1503"],
        };

        var args = BuildArguments(options);

        await Assert.That(args).IsEquivalentTo(new[]
        {
            "--exclude-diagnostics", "CS0246",
            "--exclude-diagnostics", "CS1503",
        });
    }
}
