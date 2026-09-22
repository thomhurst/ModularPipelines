using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("Enabled", null, false, true)]
    [Arguments("Enabled", false, false, true)]
    [Arguments("Enabled", true, false, false)]
    [Arguments("Enabled", true, true, true)]
    [Arguments("Enabled", null, true, true)]
    [Arguments("Clone", false, false, true)]
    [Arguments("Clone", true, false, false)]
    [Arguments("Clone", true, true, true)]
    public async Task Conditional_Requirement_Activates_Only_For_A_Present_Flag(string triggerName, bool? enabled, bool supplied, bool valid)
    {
        var generated = await Generate(
        [
            new() { SwitchName = "--enabled", PropertyName = triggerName, CSharpType = "bool?", IsFlag = true },
            new() { SwitchName = "--value", PropertyName = "Value", CSharpType = "string?" },
        ], alternativeGroups:
        [
            new CliRequiredAlternativeGroup
            {
                RequiredWhen = new() { PropertyName = triggerName, OptionSwitch = "--enabled" },
                IsChoice = false,
                Members = [new() { PropertyName = "Value", OptionSwitch = "--value", IsRequired = true }],
            },
        ]);
        var type = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(type)!;
        type.GetProperties().Single(property => property.PropertyType == typeof(bool?)).SetValue(instance, enabled);
        type.GetProperty("Value")!.SetValue(instance, supplied ? "value" : null);
        await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsEqualTo(valid);
    }
}
