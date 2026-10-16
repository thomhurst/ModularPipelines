using System.Reflection;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Required_String_Override_Shares_Nullable_Global_Storage()
    {
        var global = new CliOptionDefinition
        {
            SwitchName = "--project",
            ShortForm = "-p",
            PropertyName = "Project",
            CSharpType = "string?",
        };
        var local = global with { ShortForm = null, IsRequired = true, Description = "The command-specific project selector." };
        var command = new CliCommandDefinition
        {
            FullCommand = "tool run",
            CommandParts = ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [local],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            GlobalOptionsBeforeSubcommands = false,
            GlobalOptions = [global],
            Commands = [command],
        };
        var baseSource = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        var source = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(source).Contains("The command-specific project selector.");
        await Assert.That(source).DoesNotContain("ShortForm = \"-p\"");
        var assembly = Compile(baseSource, source);
        var baseType = assembly.GetType("ModularPipelines.Tool.Options.ToolOptions")!;
        var derivedType = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var baseProperty = baseType.GetProperty("Project")!;
        var derivedProperty = derivedType.GetProperty("Project", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        var instance = Activator.CreateInstance(derivedType, ["constructor"]);
        await Assert.That(baseProperty.GetValue(instance)).IsEqualTo("constructor");
        baseProperty.SetValue(instance, "base");
        await Assert.That(derivedProperty.GetValue(instance)).IsEqualTo("base");
        derivedProperty.SetValue(instance, "derived");
        await Assert.That(baseProperty.GetValue(instance)).IsEqualTo("derived");
        await Assert.That(() => Activator.CreateInstance(derivedType, [null]))
            .Throws<TargetInvocationException>().WithInnerException<ArgumentNullException>();
    }
}
