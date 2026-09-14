namespace ModularPipelines.OptionsGenerator.Tests;

public class CommandTimeoutTests
{
    [Test]
    [Arguments("1")]
    [Arguments("180")]
    [Arguments("600")]
    public async Task Accepts_Bounded_Command_Timeout(string seconds)
    {
        var result = await OptionsGeneratorCommand.RunAsync(["--list-tools", "--json", "--command-timeout-seconds", seconds]);

        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    [Arguments("0")]
    [Arguments("-1")]
    [Arguments("601")]
    public async Task Rejects_Out_Of_Range_Command_Timeout(string seconds)
    {
        var result = await OptionsGeneratorCommand.RunAsync(["--list-tools", "--json", "--command-timeout-seconds", seconds]);

        await Assert.That(result).IsNotEqualTo(0);
    }
}
