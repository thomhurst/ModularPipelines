using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ExecutableOverrideValidatorTests
{
    [Test]
    [Arguments("all")]
    [Arguments("helm,docker")]
    public async Task Rejects_Override_For_Multi_Tool_Invocation(string tools)
    {
        void Validate() => ExecutableOverrideValidator.Validate(tools, "/tmp/helm");

        await Assert.That(Validate)
            .Throws<InvalidOperationException>()
            .WithMessageContaining(ProcessCliCommandExecutor.ExecutableOverrideVariableName);
    }

    [Test]
    public void Allows_Override_For_One_Tool()
    {
        ExecutableOverrideValidator.Validate("helm", "/tmp/helm");
    }

    [Test]
    public void Allows_Multi_Tool_Invocation_Without_Override()
    {
        ExecutableOverrideValidator.Validate("helm,docker", null);
    }
}
