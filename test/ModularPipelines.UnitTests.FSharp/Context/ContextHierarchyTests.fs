namespace ModularPipelines.UnitTests.FSharp.Context

open ModularPipelines.Context
open ModularPipelines.Context.Domains
open ModularPipelines.Logging
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type ContextHierarchyTests() =
    [<Test>]
    member _.IModuleContext_ShouldInheritFromIPipelineContext() = async {
        let interfaces = typeof<IModuleContext>.GetInterfaces()
        do! check(Assert.That(interfaces |> Array.contains typeof<IPipelineContext>).IsTrue())
    }

    [<Test>]
    member _.IPipelineHookContext_ShouldInheritFromIPipelineContext() = async {
        let interfaces = typeof<IPipelineHookContext>.GetInterfaces()
        do! check(Assert.That(interfaces |> Array.contains typeof<IPipelineContext>).IsTrue())
    }

    [<Test>]
    member _.IPipelineContext_ShouldHaveExpectedDomainProperties() = async {
        let contextType = typeof<IPipelineContext>
        let assertProperty name expectedType = async {
            let propertyInfo = contextType.GetProperty(name)
            do! check(Assert.That(propertyInfo).IsNotNull())
            do! check(Assert.That(propertyInfo.PropertyType = expectedType).IsTrue())
        }

        do! assertProperty "Logger" typeof<IModuleLogger>
        do! assertProperty "Shell" typeof<IShellContext>
        do! assertProperty "Files" typeof<IFilesContext>
        do! assertProperty "Data" typeof<IDataContext>
        do! assertProperty "Environment" typeof<IEnvironmentDomainContext>
        do! assertProperty "Installers" typeof<IInstallersContext>
        do! assertProperty "Network" typeof<INetworkContext>
        do! assertProperty "Security" typeof<ISecurityContext>
        do! assertProperty "Services" typeof<IServicesContext>
    }

    [<Test>]
    member _.IModuleContext_ShouldHaveGetModuleMethods() = async {
        let moduleContextType = typeof<IModuleContext>
        let getModuleMethods = moduleContextType.GetMethods() |> Array.filter (fun methodInfo -> methodInfo.Name = "GetModule")
        let getModuleIfRegisteredMethods = moduleContextType.GetMethods() |> Array.filter (fun methodInfo -> methodInfo.Name = "GetModuleIfRegistered")

        do! check(Assert.That(getModuleMethods.Length >= 1).IsTrue())
        do! check(Assert.That(getModuleIfRegisteredMethods.Length >= 1).IsTrue())
    }

    [<Test>]
    member _.IModuleContext_ShouldHaveSubModuleMethods() = async {
        let moduleContextType = typeof<IModuleContext>
        let subModuleMethods = moduleContextType.GetMethods() |> Array.filter (fun methodInfo -> methodInfo.Name = "SubModule")

        do! check(Assert.That(subModuleMethods.Length >= 1).IsTrue())
    }
