using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Generated_Options_Printing_Redacts_Local_Inherited_And_Positional_Secrets(
        bool required, bool alternateInput)
    {
        List<CliOptionDefinition> options =
        [
            new() { SwitchName = "--password", PropertyName = "Password", CSharpType = "string?", IsSecret = true, IsRequired = required },
            new() { SwitchName = "--tokens", PropertyName = "Tokens", CSharpType = "IEnumerable<string>?", IsSecret = true },
        ];
        if (alternateInput)
        {
            options.Add(new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" });
        }

        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "output",
            GlobalOptions = [new() { SwitchName = "--token", PropertyName = "Token", CSharpType = "string?", IsSecret = true }],
            Commands = [new()
            {
                FullCommand = "tool run", CommandParts = ["run"], ClassName = "ToolRunOptions", ParentClassName = "ToolOptions",
                ToolNamespacePrefix = "Tool", Options = options,
                PositionalArguments = [new() { PropertyName = "Credential", CSharpType = "string?", IsSecret = true }],
            }],
        };
        var globalSource = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        var commandSource = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        const string support = """
            namespace ModularPipelines.Secrets
            {
                public sealed class SecretValueAttribute(params string[] keys) : System.Attribute;
            }
            namespace ModularPipelines.Tool.Options
            {
                public record GlobalOnlyOptions : ToolOptions;
                public sealed class SecretCollection : System.Collections.Generic.List<string>
                {
                    public override string ToString() => "collection-secret";
                }
            }
            """;
        var assembly = Compile(globalSource, commandSource, support);
        var type = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(type, required ? ["password-secret"] : [])!;
        if (!required)
        {
            type.GetProperty("Password")!.SetValue(instance, "password-secret");
        }

        type.GetProperty("Credential")!.SetValue(instance, "positional-secret");
        type.GetProperty("Token")!.SetValue(instance, "global-secret");
        type.GetProperty("Tokens")!.SetValue(instance,
            Activator.CreateInstance(assembly.GetType("ModularPipelines.Tool.Options.SecretCollection")!));

        var printed = instance.ToString()!;
        await Assert.That(printed).DoesNotContain("secret");
        await Assert.That(printed).Contains("<redacted>");
        await Assert.That(type.GetProperty("Password")!.GetValue(instance)).IsEqualTo("password-secret");

        var globalType = assembly.GetType("ModularPipelines.Tool.Options.GlobalOnlyOptions")!;
        var globalInstance = Activator.CreateInstance(globalType)!;
        globalType.GetProperty("Token")!.SetValue(globalInstance, "global-secret");
        await Assert.That(globalInstance.ToString()!).DoesNotContain("global-secret");
        await Assert.That(globalInstance.ToString()!).Contains("<redacted>");
    }
}
