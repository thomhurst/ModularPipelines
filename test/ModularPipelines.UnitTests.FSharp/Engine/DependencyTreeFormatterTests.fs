namespace ModularPipelines.UnitTests.FSharp.Engine

open System
open System.IO
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Models
open ModularPipelines.Modules
open Spectre.Console
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private ModuleA() =
    inherit Module<bool>()
    override _.ExecuteAsync(_: IModuleContext, _) = System.Threading.Tasks.Task.FromResult(true)

type private ModuleB() =
    inherit Module<bool>()
    override _.ExecuteAsync(_: IModuleContext, _) = System.Threading.Tasks.Task.FromResult(true)

type private ModuleC() =
    inherit Module<bool>()
    override _.ExecuteAsync(_: IModuleContext, _) = System.Threading.Tasks.Task.FromResult(true)

type private ModuleD() =
    inherit Module<bool>()
    override _.ExecuteAsync(_: IModuleContext, _) = System.Threading.Tasks.Task.FromResult(true)

type private SharedModule() =
    inherit Module<bool>()
    override _.ExecuteAsync(_: IModuleContext, _) = System.Threading.Tasks.Task.FromResult(true)

[<AutoOpen>]
module private DependencyTreeFormatterTestHelpers =
    let renderToString (tree: Tree) =
        use writer = new StringWriter()
        let settings = AnsiConsoleSettings()
        settings.Out <- AnsiConsoleOutput(writer)
        settings.Ansi <- AnsiSupport.No
        let console = AnsiConsole.Create(settings)
        console.Write(tree)
        writer.ToString()

    let createModel<'T when 'T :> IModule and 'T : (new: unit -> 'T)> () =
        ModuleDependencyModel(Activator.CreateInstance<'T>())

type DependencyTreeFormatterTests() =
    [<Test>]
    member _.FormatTree_SingleModule_NoDependencies_ReturnsTreeWithSingleNode() = async {
        let formatter = DependencyTreeFormatter()
        let moduleA = createModel<ModuleA>()
        let roots = [| moduleA |]

        let tree = formatter.FormatTree(roots)
        let output = renderToString tree

        do! check(Assert.That(output.Contains("Module Dependencies")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleA")).IsTrue())
    }

    [<Test>]
    member _.FormatTree_LinearChain_ReturnsTreeWithCorrectHierarchy() = async {
        let formatter = DependencyTreeFormatter()
        let moduleA = createModel<ModuleA>()
        let moduleB = createModel<ModuleB>()
        let moduleC = createModel<ModuleC>()

        moduleA.IsDependencyFor.Add(moduleB)
        moduleB.IsDependencyFor.Add(moduleC)

        let tree = formatter.FormatTree([| moduleA |])
        let output = renderToString tree

        do! check(Assert.That(output.Contains("ModuleA")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleB")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleC")).IsTrue())
    }

    [<Test>]
    member _.FormatTree_MultipleRoots_ReturnsTreeWithAllRoots() = async {
        let formatter = DependencyTreeFormatter()
        let moduleA = createModel<ModuleA>()
        let moduleB = createModel<ModuleB>()

        let tree = formatter.FormatTree([| moduleA; moduleB |])
        let output = renderToString tree

        do! check(Assert.That(output.Contains("ModuleA")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleB")).IsTrue())
    }

    [<Test>]
    member _.FormatTree_SharedModule_MarkedAsReference_OnSecondOccurrence() = async {
        let formatter = DependencyTreeFormatter()
        let moduleA = createModel<ModuleA>()
        let moduleB = createModel<ModuleB>()
        let shared = createModel<SharedModule>()

        moduleA.IsDependencyFor.Add(shared)
        moduleB.IsDependencyFor.Add(shared)

        let tree = formatter.FormatTree([| moduleA; moduleB |])
        let output = renderToString tree

        do! check(Assert.That(output.Contains("SharedModule")).IsTrue())
        do! check(Assert.That(output.Contains("(↑)")).IsTrue())
    }

    [<Test>]
    member _.FormatTree_DiamondDependency_ShowsReferenceMarkerForSharedLeaf() = async {
        let formatter = DependencyTreeFormatter()
        let moduleA = createModel<ModuleA>()
        let moduleB = createModel<ModuleB>()
        let moduleC = createModel<ModuleC>()
        let moduleD = createModel<ModuleD>()

        moduleA.IsDependencyFor.Add(moduleB)
        moduleA.IsDependencyFor.Add(moduleC)
        moduleB.IsDependencyFor.Add(moduleD)
        moduleC.IsDependencyFor.Add(moduleD)

        let tree = formatter.FormatTree([| moduleA |])
        let output = renderToString tree

        do! check(Assert.That(output.Contains("ModuleA")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleB")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleC")).IsTrue())
        do! check(Assert.That(output.Contains("ModuleD")).IsTrue())
        do! check(Assert.That(output.Contains("(↑)")).IsTrue())
    }

    [<Test>]
    member _.FormatTree_EmptyCollection_OnlyContainsHeader() = async {
        let formatter = DependencyTreeFormatter()
        let roots = Array.empty<ModuleDependencyModel>

        let tree = formatter.FormatTree(roots)
        let output = renderToString tree

        do! check(Assert.That(output.Contains("Module Dependencies")).IsTrue())
        do! check(Assert.That(output.Contains("├")).IsFalse())
        do! check(Assert.That(output.Contains("└")).IsFalse())
    }

    [<Test>]
    member _.FormatTree_AlreadyPrintedRoot_SkipsIt() = async {
        let formatter = DependencyTreeFormatter()
        let moduleA = createModel<ModuleA>()

        let tree = formatter.FormatTree([| moduleA; moduleA |])
        let output = renderToString tree
        let count = output.Split([| "ModuleA" |], StringSplitOptions.None).Length - 1
        
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(count), 1))
    }
