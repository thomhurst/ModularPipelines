using Pipeline.NET.Exceptions;

namespace Pipeline.NET.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task OneSecondModule_WillWaitForFiveSecondModule_ThenExecute()
    {
        var modules = await PipelineHostBuilder.Create()
            .AddModule<FiveSecondModule>()
            .AddModule<OneSecondModuleDependentOnFiveSecondModule>()
            .ExecutePipelineAsync();

        var fiveSecondModule = modules.OfType<FiveSecondModule>().Single();
        var oneSecondModuleDependentOnFiveSecondModule = modules.OfType<OneSecondModuleDependentOnFiveSecondModule>().Single();
        
        Assert.Multiple(() =>
        {

            // 5 + 1
            Assert.That(oneSecondModuleDependentOnFiveSecondModule.Duration, Is.GreaterThanOrEqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(oneSecondModuleDependentOnFiveSecondModule.EndTime, Is.GreaterThanOrEqualTo(fiveSecondModule.StartTime + TimeSpan.FromSeconds(6)));
            Assert.That(oneSecondModuleDependentOnFiveSecondModule.StartTime, Is.GreaterThanOrEqualTo(fiveSecondModule.EndTime));
        });
    }
    
    [Test]
    public void Modules_Dependent_On_Each_Other_Throws_Exception()
    {
        Assert.That(() => PipelineHostBuilder.Create()
            .AddModule<DependencyConflictModule1>()
            .AddModule<DependencyConflictModule2>()
            .ExecutePipelineAsync(), Throws.Exception.TypeOf<DependencyCollisionException>());
    }
}