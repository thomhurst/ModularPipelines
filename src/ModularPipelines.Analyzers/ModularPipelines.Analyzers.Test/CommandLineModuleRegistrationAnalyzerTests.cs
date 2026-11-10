using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CommandLineModuleRegistrationAnalyzerTests
{
    [TestMethod]
    [DataRow("Program", "return await command.Parse(args).InvokeAsync();")]
    [DataRow("BuildModule", "return await command.Parse(args).InvokeAsync();")]
    [DataRow("BuildModule", "var result = command.Parse(args); return await result.InvokeAsync();")]
    [DataRow("BuildModule", "var alias = command; return await alias.Parse(args).InvokeAsync();")]
    public async Task Recognizes_Assembly_Registration_In_Invoked_Action(string assemblyType, string invocation)
    {
        var source = Source($$"""
            command.SetAction(async _ =>
            {
                var builder = Pipeline.CreateBuilder();
                builder.AddModulesFromAssembly(typeof({{assemblyType}}).Assembly);
                await using var pipeline = await builder.BuildAsync();
            });
            {{invocation}}
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Module_Registration_In_Synchronous_Action()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            return command.Parse(args).Invoke();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("command.Parse(args);")]
    [DataRow("await new System.CommandLine.RootCommand().Parse(args).InvokeAsync();")]
    [DataRow("command = new System.CommandLine.RootCommand(); await command.Parse(args).InvokeAsync();")]
    [DataRow("Func<Task<int>> invoke = () => command.Parse(args).InvokeAsync();")]
    public async Task Reports_Module_Registered_Only_In_Uninvoked_Action(string invocation)
    {
        var source = Source($$"""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            {{invocation}}
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    public async Task Reports_Module_When_Action_Is_Overwritten()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            command.SetAction(_ => { });
            return command.Parse(args).Invoke();
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); } else { command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); }", true)]
    [DataRow("if (args.Length > 0) { if (args.Length > 1) { command.SetAction(_ => { }); } else { command.SetAction(_ => { }); } } else { command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); } else { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); }", true)]
    public async Task Tracks_Action_Replacement_Across_Branches(string replacement, bool registered)
    {
        var source = Source($$"""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            {{replacement}}
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => { }); } else { command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { }); } else { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => { }); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); if (args.Length > 1) { command.SetAction(_ => { }); } else { command.SetAction(_ => { }); } }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); if (args.Length > 1) { command.SetAction(_ => { }); } }", true)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.SetAction(_ => { }); } else { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); }", true)]
    public async Task Tracks_Action_Replacement_Within_Branches(string statements, bool registered)
    {
        var source = Source($$"""
            {{statements}}
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.SetAction(_ => { }); command.Parse(args).Invoke(); }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.Parse(args).Invoke(); }", false)]
    [DataRow("if (args.Length > 1) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { if (args.Length > 0) { command.Parse(args).Invoke(); } }", false)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); command.Parse(args).Invoke(); }", true)]
    [DataRow("if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } command.Parse(args).Invoke();", true)]
    [DataRow("foreach (var arg in args) { if (arg.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.Parse(args).Invoke(); } }", true)]
    [DataRow("foreach (var arg in args) { if (arg.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.SetAction(_ => { }); command.Parse(args).Invoke(); } }", false)]
    [DataRow("do { if (args.Length > 0) { command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); }); } else { command.Parse(args).Invoke(); } } while (false);", false)]
    public async Task Tracks_Actions_On_Paths_Compatible_With_Invocation(string statements, bool registered)
    {
        await VerifyAsync(Source(statements, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("return 0;", false)]
    [DataRow("throw new InvalidOperationException();", false)]
    [DataRow("if (args.Length > 1) return 0; else return 1;", false)]
    [DataRow("if (args.Length > 1) return 0;", true)]
    [DataRow("Action exit = () => { return; }; exit();", true)]
    [DataRow("goto Invoke;", true)]
    public async Task Tracks_Actions_In_Branches_That_Can_Reach_Invocation(string exit, bool registered)
    {
        var source = Source($$"""
            if (args.Length > 0)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{exit}}
            }
            Invoke:
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("return 0;", "finally")]
    [DataRow("throw new InvalidOperationException();", "catch (InvalidOperationException)")]
    public async Task Recognizes_Action_Invoked_By_Exit_Handler(string exit, string handler)
    {
        var source = Source($$"""
            try
            {
                if (args.Length > 0)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    {{exit}}
                }
            }
            {{handler}}
            {
                command.Parse(args).Invoke();
            }
            return 0;
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("return 0;", false)]
    [DataRow("throw;", false)]
    [DataRow("command.SetAction(_ => { });", false)]
    public async Task Tracks_Actions_After_Caught_Exception(string handler, bool registered)
    {
        var source = Source($$"""
            try
            {
                if (args.Length > 0)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                {{handler}}
            }
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_After_Unconditional_Caught_Exception()
    {
        await VerifyAsync(Source("""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException) { }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow("InvalidOperationException", "", true)]
    [DataRow("ArgumentException", "", false)]
    [DataRow("InvalidOperationException", "when (true)", true)]
    [DataRow("InvalidOperationException", "when (false)", false)]
    [DataRow("ArgumentException", "when (true)", false)]
    public async Task Matches_Exception_Handler_Before_Later_Invocation(string caught, string filter, bool registered)
    {
        var source = Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw new InvalidOperationException();
            }
            catch ({{caught}}) {{filter}} { }
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("command.SetAction(_ => { });", false)]
    [DataRow("throw new InvalidOperationException();", false)]
    public async Task Tracks_Action_Through_Finally_Before_Later_Invocation(string cleanup, bool registered)
    {
        var source = Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            }
            finally { {{cleanup}} }
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_Invoked_In_Filter_Before_Finally_Replacement()
    {
        await VerifyAsync(Source("""
            try
            {
                try
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
                finally { command.SetAction(_ => { }); }
            }
            catch (InvalidOperationException) when (command.Parse(args).Invoke() == 0) { }
            """));
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Thrown_Variable_Has_Base_Type()
    {
        await VerifyAsync(Source("""
            Exception error = new InvalidOperationException();
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw error;
            }
            catch (InvalidOperationException) { }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow("double", "1.0", "/", false)]
    [DataRow("double", "1.0", "%", false)]
    [DataRow("float", "1.0f", "/", false)]
    [DataRow("float", "1.0f", "%", false)]
    [DataRow("double?", "1.0", "/", false)]
    [DataRow("double", "1.0", "+", false)]
    [DataRow("int", "1", "/", true)]
    [DataRow("int", "1", "%", true)]
    [DataRow("int", "1", "+", true)]
    [DataRow("decimal", "1.0m", "/", true)]
    public async Task Tracks_Only_Throwing_Binary_Operations(string type, string value, string operation, bool registered)
    {
        await VerifyAsync(Source($$"""
            {{type}} divisor = 0;
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            try
            {
                _ = checked({{value}} {{operation}} divisor);
                command.SetAction(_ => { });
            }
            catch (Exception) { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = holder.Value;", true)]
    [DataRow("holder.Value = 1;", true)]
    [DataRow("_ = holder.Child.Value;", true)]
    [DataRow("_ = Holder.StaticValue;", false)]
    [DataRow("Holder.StaticValue = 1;", false)]
    [DataRow("_ = Holder.Constant;", false)]
    [DataRow("_ = valueHolder.Value;", false)]
    [DataRow("valueHolder.Value = 1;", false)]
    public async Task Tracks_Field_Access_Exception_Paths(string access, bool registered)
    {
        await VerifyAsync(Source($$"""
            Holder holder = null!;
            ValueHolder valueHolder = default;
            Action register = () => {};
            command.SetAction(parseResult =>
            {
                try
                {
                    register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    {{access}}
                    register = () => {};
                }
                catch (NullReferenceException) { }
                register();
            });
            return command.Parse(args).Invoke();

            public class Holder
            {
                public int Value;
                public Holder Child = null!;
                public static int StaticValue;
                public const int Constant = 0;
            }

            public struct ValueHolder
            {
                public int Value;
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(true, true)]
    public async Task Field_Write_Evaluates_Callback_Value_Before_Null_Check(bool initiallyRegisters, bool replacementRegisters)
    {
        await VerifyAsync(Source($$"""
            Holder holder = null!;
            Action register = {{(initiallyRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            command.SetAction(parseResult =>
            {
                try
                {
                    holder.Callback = register = {{(replacementRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                    register = () => {};
                }
                catch (NullReferenceException) { }
                register();
            });
            return command.Parse(args).Invoke();

            public class Holder
            {
                public Action Callback = null!;
            }
            """, replacementRegisters), replacementRegisters ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("Value")]
    [DataRow("this.Value")]
    [DataRow("base.Value")]
    public async Task Does_Not_Add_Null_Exception_Path_For_Current_Instance_Field(string access)
    {
        await VerifyAsync(Source($$"""
            return new Holder().Run(command, args);

            public class BaseHolder
            {
                public int Value;
            }

            public class Holder : BaseHolder
            {
                public int Run(System.CommandLine.RootCommand command, string[] args)
                {
                    Action register = () => {};
                    command.SetAction(parseResult =>
                    {
                        try
                        {
                            register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                            _ = {{access}};
                            register = () => {};
                        }
                        catch (NullReferenceException) { }
                        register();
                    });
                    return command.Parse(args).Invoke();
                }
            }
            """, registered: false), UnregisteredModule());
    }

    [TestMethod]
    [DataRow("_ = holder.Value;")]
    [DataRow("holder.Value = 1;")]
    public async Task Preserves_Command_Action_After_Caught_Field_Access(string access)
    {
        await VerifyAsync(Source($$"""
            Holder holder = null!;
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{access}}
                command.SetAction(_ => { });
            }
            catch (NullReferenceException) { }
            return command.Parse(args).Invoke();

            public class Holder
            {
                public int Value;
            }
            """));
    }

    [TestMethod]
    [DataRow("new Holder()", "NullReferenceException", false)]
    [DataRow("null!", "InvalidOperationException", false)]
    [DataRow("null!", "NullReferenceException", true)]
    public async Task Tracks_Field_Exception_Paths_For_Callback_Definitions(string receiver, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            command.SetAction(parseResult =>
            {
                Holder holder = {{receiver}};
                Action register = () => {};
                try { _ = holder.Value; }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            });
            return command.Parse(args).Invoke();

            public class Holder { public int Value; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = args[0];", "IndexOutOfRangeException")]
    [DataRow("_ = System.Environment.CurrentDirectory;", "Exception")]
    [DataRow("_ = new object();", "Exception")]
    [DataRow("_ = new int[args.Length];", "Exception")]
    [DataRow("_ = 1 / args.Length;", "DivideByZeroException")]
    [DataRow("_ = checked(args.Length + 1);", "OverflowException")]
    [DataRow("_ = (string)(object)args;", "InvalidCastException")]
    public async Task Preserves_Action_When_Operation_Throws_Before_Replacement(string operation, string caught)
    {
        await VerifyAsync(Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{operation}}
                command.SetAction(_ => { });
            }
            catch ({{caught}}) { }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow("var holder = new Holder();", "_ = holder.Value;", false)]
    [DataRow("var holder = new Holder();", "holder.Value = 1;", false)]
    [DataRow("var original = new Holder(); var holder = original;", "_ = holder.Value;", false)]
    [DataRow("Holder holder; if (args.Length > 0) holder = new Holder(); else holder = new Holder();", "_ = holder.Value;", false)]
    [DataRow("var holder = new Holder(); holder = null!;", "_ = holder.Value;", true)]
    [DataRow("Holder holder = null!; holder = new Holder();", "_ = holder.Value;", false)]
    [DataRow("var holder = new Holder(); if (args.Length > 0) holder = null!;", "_ = holder.Value;", true)]
    [DataRow("var holder = new Holder(); Clear(ref holder); static void Clear(ref Holder value) { value = null!; }", "_ = holder.Value;", true)]
    [DataRow("var holder = new Holder(); Action clear = () => holder = null!; clear();", "_ = holder.Value;", true)]
    [DataRow("Holder holder = new Holder(); ref Holder alias = ref holder; alias = null!;", "_ = holder.Value;", true)]
    [DataRow("Holder holder = new Holder(); (holder, _) = ((Holder)null!, 1);", "_ = holder.Value;", true)]
    [DataRow("Holder holder = new Holder(); Holder other = new Holder(); ref Holder alias = ref other; alias = ref holder; alias = null!;", "_ = holder.Value;", true)]
    [DataRow("Holder holder = new Holder(); Holder other = new Holder(); ref Holder alias = ref other; alias = ref holder; alias = null!;", "holder.Value = 1;", true)]
    public async Task Tracks_Known_NonNull_Field_Receivers(string setup, string access, bool registered)
    {
        await VerifyAsync(Source($$"""
            {{setup}}
            command.SetAction(_ => {});
            try
            {
                {{access}}
            }
            catch (NullReferenceException)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            }
            return command.Parse(args).Invoke();

            public class Holder { public int Value; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = holder.Value;", "InvalidOperationException", false)]
    [DataRow("holder.Value = 1;", "InvalidOperationException", false)]
    [DataRow("_ = holder.Value;", "NullReferenceException", true)]
    [DataRow("holder.Value = 1;", "SystemException", true)]
    [DataRow("_ = holder.Value;", "Exception", true)]
    public async Task Matches_Field_Access_Exception_Type(string access, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            Holder holder = null!;
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{access}}
                command.SetAction(_ => {});
            }
            catch ({{caught}}) { }
            return command.Parse(args).Invoke();

            public class Holder { public int Value; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = args[0];", "IndexOutOfRangeException")]
    [DataRow("_ = System.Environment.CurrentDirectory;", "Exception")]
    [DataRow("_ = new object();", "Exception")]
    public async Task Recognizes_Action_Invoked_In_Operation_Exception_Handler(string operation, string caught)
    {
        await VerifyAsync(Source($$"""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            try { {{operation}} }
            catch ({{caught}}) { command.Parse(args).Invoke(); }
            """));
    }

    [TestMethod]
    [DataRow("catch", false)]
    [DataRow("catch (Exception)", false)]
    [DataRow("catch (InvalidOperationException)", true)]
    [DataRow("catch (Exception) when (true)", false)]
    [DataRow("catch (Exception) when (false)", true)]
    public async Task Stops_Exception_Search_After_Catch_All(string handler, bool registered)
    {
        await VerifyAsync(Source($$"""
            try
            {
                try
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    System.GC.KeepAlive(args);
                    command.SetAction(_ => { });
                }
                {{handler}} { return 0; }
            }
            catch { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("true", false)]
    [DataRow("false", true)]
    [DataRow("args.Length > 0", true)]
    public async Task Routes_Rejected_Filter_To_Later_Handler(string filter, bool registered)
    {
        await VerifyAsync(Source($$"""
            try
            {
                try
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
                catch (InvalidOperationException) when ({{filter}}) { command.SetAction(_ => { }); }
            }
            catch { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("command.SetAction(_ => { });", false)]
    public async Task Preserves_Finally_Destination_Through_Nested_Catch(string replacement, bool registered)
    {
        await VerifyAsync(Source($$"""
            try { }
            finally
            {
                try { throw new InvalidOperationException(); }
                catch (InvalidOperationException)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                }
                {{replacement}}
            }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("true", false)]
    [DataRow("false", true)]
    public async Task Routes_Rejected_Filter_To_Sibling_Handler(string filter, bool registered)
    {
        await VerifyAsync(Source($$"""
            try
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException) when ({{filter}}) { command.SetAction(_ => { }); }
            catch (Exception) { }
            return command.Parse(args).Invoke();
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Preserves_Outer_Finally_Destination_When_Nested_Finally_Throws()
    {
        await VerifyAsync(Source("""
            try { }
            finally
            {
                try
                {
                    try { } finally { throw new InvalidOperationException(); }
                }
                catch (InvalidOperationException)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                }
            }
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    public async Task Discards_Finally_Destination_When_Exception_Escapes()
    {
        await VerifyAsync(Source("""
            try
            {
                try { }
                finally
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    throw new InvalidOperationException();
                }
                command.Parse(args).Invoke();
            }
            catch (InvalidOperationException) { }
            """, registered: false), UnregisteredModule());
    }

    [TestMethod]
    [DataRow("for (var i = 0; i < 2; i++)", "", "", true)]
    [DataRow("for (var i = 0; i < 2; i++)", "", "break;", false)]
    [DataRow("for (var i = 0; i < 2; i++)", "", "return 0;", false)]
    [DataRow("for (var i = 0; i < 2; i++)", "command.SetAction(_ => { });", "", false)]
    [DataRow("for (var i = 0; i < 2; i++)", "", "command.SetAction(_ => { });", false)]
    [DataRow("do", "", "", false)]
    [DataRow("do", "", "if (args.Length > 0) goto Again;", true)]
    public async Task Tracks_Actions_Installed_After_Invocation(string loop, string beforeInvocation, string afterSetter, bool registered)
    {
        var source = Source($$"""
            command.SetAction(_ => { });
            {{loop}}
            {
                Again:
                {{beforeInvocation}}
                command.Parse(args).Invoke();
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{afterSetter}}
            } {{(loop == "do" ? "while (false);" : string.Empty)}}
            return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("var freshCommand = new System.CommandLine.RootCommand();", "freshCommand", "", false)]
    [DataRow("var freshCommand = new System.CommandLine.RootCommand(); var alias = freshCommand;", "alias", "", false)]
    [DataRow("command = new System.CommandLine.RootCommand();", "command", "", false)]
    [DataRow("var freshCommand = new System.CommandLine.RootCommand();", "freshCommand", "freshCommand.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });", true)]
    [DataRow("var freshCommand = command;", "freshCommand", "", true)]
    public async Task Resets_Actions_When_Loop_Creates_Command(string declaration, string receiver, string beforeInvocation, bool registered)
    {
        await VerifyAsync(Source($$"""
            for (var i = 0; i < 2; i++)
            {
                {{declaration}}
                {{beforeInvocation}}
                {{receiver}}.Parse(args).Invoke();
                {{receiver}}.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            }
            return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_Invoked_Before_Replacement_In_Loop()
    {
        await VerifyAsync(Source("""
            for (var i = 0; i < 2; i++)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                command.Parse(args).Invoke();
                command.SetAction(_ => { });
            }
            """));
    }

    [TestMethod]
    [DataRow("Action run = () =>", ";")]
    [DataRow("void run()", "")]
    public async Task Recognizes_Action_In_Invoked_Nested_Callable(string declaration, string terminator)
    {
        await VerifyAsync(Source($$"""
            {{declaration}}
            {
                var nestedCommand = new System.CommandLine.RootCommand();
                nestedCommand.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                nestedCommand.Parse(args).Invoke();
            }{{terminator}}
            run();
            """));
    }

    [TestMethod]
    [DataRow("public static Func<int> Run =", true)]
    [DataRow("public static Func<int> Run { get; } =", true)]
    [DataRow("public static Func<int> Run =", false)]
    [DataRow("public static Func<int> Run { get; } =", false)]
    public async Task Tracks_Action_In_Initializer_Lambda(string declaration, bool invoked)
    {
        var source = Source(invoked ? "return Startup.Run();" : "return 0;", registered: invoked) + $$"""

            public static class Startup
            {
                {{declaration}} () =>
                {
                    var command = new System.CommandLine.RootCommand();
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                    return command.Parse(Array.Empty<string>()).Invoke();
                };
            }
            """;

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("Replace:", false)]
    [DataRow("Replace: Another:", false)]
    [DataRow("", true)]
    public async Task Tracks_Jumps_To_Or_Past_Action_Replacement(string replacementLabel, bool registered)
    {
        var source = Source($$"""
            do
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                if (args.Length > 0) goto Replace;
                {{replacementLabel}}
                command.SetAction(_ => { });
                {{(registered ? "Replace: ;" : string.Empty)}}
            } while (false);
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("if (args.Length > 0) break;", true)]
    [DataRow("if (args.Length > 0) continue;", true)]
    [DataRow("while (args.Length > 0) { break; }", false)]
    [DataRow("Action skip = () => { return; }; skip();", false)]
    public async Task Tracks_Action_Replacement_Around_Loop_Exits(string interveningStatements, bool registered)
    {
        var source = Source($$"""
            do
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{interveningStatements}}
                command.SetAction(_ => { });
            } while (false);
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Jump_Skips_Replacement()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            if (args.Length > 0) goto Invoke;
            command.SetAction(_ => { });
            Invoke:
            return command.Parse(args).Invoke();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Jump_Enters_Invoked_Branch_After_Replacement()
    {
        var source = Source("""
            foreach (var arg in args)
            {
                if (arg.Length > 0)
                {
                    command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                }
                else
                {
                    if (args.Length > 2) goto Invoke;
                    command.SetAction(_ => { });
                    Invoke:
                    command.Parse(args).Invoke();
                }
            }
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Branch_Exits_Before_Replacement()
    {
        var source = Source("""
            do
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                if (args.Length > 0)
                {
                    if (args.Length > 1) break;
                    command.SetAction(_ => { });
                }
                else
                {
                    command.SetAction(_ => { });
                }
            } while (false);
            return command.Parse(args).Invoke();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Action_When_Backward_Jump_Reenters_Opposite_Branch()
    {
        var source = Source("""
            var register = args.Length > 0;
            Again:
            if (register)
            {
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                register = false;
                goto Again;
            }
            else
            {
                command.Parse(args).Invoke();
            }
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            command.SetAction(_ =>
            {
                var copy = register;
                {{(invoked ? "copy();" : string.Empty)}}
            });
            return command.Parse(args).Invoke();
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Member_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            command.SetAction(_ =>
            {
                var copy = Callbacks.Register;
                {{(invoked ? "copy();" : string.Empty)}}
            });
            return command.Parse(args).Invoke();
            """, registered: invoked) + """

            public static class Callbacks
            {
                public static Action Register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Nested_Callback_Invocation_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            command.SetAction(_ =>
            {
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                {{(invoked ? "register();" : string.Empty)}}
            });
            return command.Parse(args).Invoke();
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Recognizes_Action_Stored_In_Delegate_Local()
    {
        var source = Source("""
            Action<System.CommandLine.ParseResult> action = Register;
            command.SetAction(action);
            return command.Parse(args).Invoke();

            void Register(System.CommandLine.ParseResult result) =>
                Pipeline.CreateBuilder().AddModule<BuildModule>();
            """);

        await VerifyAsync(source);
    }

    [TestMethod]
    [DataRow(true, false)]
    [DataRow(false, true)]
    public async Task Tracks_Captured_Delegate_Reassigned_After_Action_Installation(bool initiallyRegistered, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegistered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            command.SetAction(_ => register());
            register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("command.SetAction(_ => { var copy = register; copy(); });")]
    [DataRow("command.SetAction(_ => { Action inner = () => register(); inner(); });")]
    [DataRow("command.SetAction(_ => { void Inner() => register(); Inner(); });")]
    public async Task Tracks_Reassigned_Delegate_Through_Nested_Callbacks(string install)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            {{install}}
            register = () => {};
            return command.Parse(args).Invoke();
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("Action action = () => register();", "action();")]
    [DataRow("void Action() => register();", "Action();")]
    public async Task Tracks_Reassigned_Delegate_Through_Direct_Callable(string declaration, string invocation)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            {{declaration}}
            register = () => {};
            {{invocation}}
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    public async Task Tracks_Captured_Delegate_At_Each_Execution()
    {
        await VerifyAsync(Source("""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            command.SetAction(_ => register());
            command.Parse(args).Invoke();
            register = () => {};
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    public async Task Preserves_Delegate_Value_Copied_Before_Reassignment()
    {
        await VerifyAsync(Source("""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            var copy = register;
            command.SetAction(_ => copy());
            register = () => {};
            return command.Parse(args).Invoke();
            """));
    }

    [TestMethod]
    [DataRow(false, false, false)]
    [DataRow(false, true, true)]
    [DataRow(true, false, true)]
    [DataRow(true, true, true)]
    public async Task Tracks_Captured_Delegate_Replaced_In_Every_Branch(bool firstRegisters, bool secondRegisters, bool registered)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            command.SetAction(_ =>
            {
                if (args.Length > 0)
                    register = {{(firstRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                else
                    register = {{(secondRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                register();
            });
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false, false, false)]
    [DataRow(false, true, true)]
    [DataRow(true, false, true)]
    [DataRow(true, true, true)]
    public async Task Tracks_Captured_Delegate_Replaced_In_One_Branch(bool initiallyRegisters, bool replacementRegisters, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            command.SetAction(_ =>
            {
                if (args.Length > 0)
                    register = {{(replacementRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                register();
            });
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("return;", false)]
    [DataRow("throw new InvalidOperationException();", false)]
    [DataRow("if (args.Length > 1) return; else return;", false)]
    [DataRow("if (args.Length > 1) return;", true)]
    [DataRow("goto End;", false)]
    [DataRow("goto Invoke;", true)]
    public async Task Excludes_Captured_Delegate_Writes_That_Cannot_Reach_Invocation(string exit, bool registered)
    {
        var source = Source($$"""
            Action register = () => {};
            command.SetAction(_ =>
            {
                if (args.Length > 0)
                {
                    register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    {{exit}}
                }
                Invoke:
                register();
                End:;
            });
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Excludes_Delegate_Writes_In_An_Exiting_Top_Level_Branch()
    {
        var source = Source("""
            Action register = () => {};
            if (args.Length > 0)
            {
                register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                return 0;
            }
            register();
            return 0;
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("try { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); throw new InvalidOperationException(); } } catch (InvalidOperationException) { } register();", false, true)]
    [DataRow("try { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return; } } finally { register(); }", false, true)]
    [DataRow("try { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return; } } finally { register = () => {}; } register();", false, false)]
    [DataRow("if (args.Length > 0) { register = () => {}; return; } register();", true, true)]
    [DataRow("do { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); continue; } register(); } while (false);", false, false)]
    [DataRow("do { if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); break; } register(); } while (false);", false, false)]
    [DataRow("while (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); break; } register();", false, true)]
    [DataRow("if (args.Length > 0) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } else { register(); }", false, false)]
    [DataRow("for (var i = 0; i < 2; i++) { register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", false, true)]
    public async Task Tracks_Captured_Delegate_Definitions_Through_Control_Flow(string statements, bool initiallyRegisters, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            command.SetAction(_ =>
            {
                {{statements}}
            });
            return command.Parse(args).Invoke();
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [Timeout(10000, CooperativeCancellation = true)]
    public async Task Terminates_Captured_Delegate_Lookup_In_Recursive_Callables(bool mutualRecursion, bool registered)
    {
        var source = Source($$"""
            Action register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            void Run(int depth)
            {
                if (depth > 0) {{(mutualRecursion ? "Other(depth - 1);" : "Run(depth - 1);")}}
                register();
            }
            {{(mutualRecursion ? "void Other(int depth) { if (depth > 0) Run(depth - 1); register(); }" : string.Empty)}}
            Run(2);
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("register = () => {}; if (args.Length > 0) register = () => {};")]
    [DataRow("switch (args.Length) { case 0: register = () => {}; break; default: register = () => {}; break; }")]
    [DataRow("if (args.Length > 0) { if (args.Length > 1) register = () => {}; else register = () => {}; } else register = () => {};")]
    public async Task Does_Not_Resurrect_Captured_Delegate_After_Guaranteed_Replacement(string replacement)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            command.SetAction(_ =>
            {
                {{replacement}}
                register();
            });
            return command.Parse(args).Invoke();
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("", "command", true)]
    [DataRow("command = new System.CommandLine.RootCommand();", "command", false)]
    [DataRow("command = new System.CommandLine.RootCommand();", "alias", true)]
    [DataRow("command = new System.CommandLine.RootCommand(); command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });", "command", true)]
    public async Task Tracks_Command_Parameter_Origins(string replacement, string receiver, bool registered)
    {
        var source = Source($$"""
            return Run(command, args);

            static int Run(System.CommandLine.RootCommand command, string[] args)
            {
                var alias = command;
                command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
                {{replacement}}
                return {{receiver}}.Parse(args).Invoke();
            }
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Invoked_Action_Does_Not_Hide_Unregistered_Module()
    {
        var source = Source("""
            command.SetAction(_ => { Pipeline.CreateBuilder().AddModule<BuildModule>(); });
            return command.Parse(args).Invoke();
            """) + $$"""

            public class {|#0:UnregisteredModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }
            """;

        await VerifyAsync(source, VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0).WithArguments("UnregisteredModule"));
    }

    [TestMethod]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return _ => register();", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return _ => register();", true, false)]
    [DataRow("Action register = () => {}; return _ => register();", false, true)]
    [DataRow("Action register = () => {}; return _ => register();", false, false)]
    [DataRow("Action register = () => {}; Action<System.CommandLine.ParseResult> action = _ => register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return action;", true, true)]
    [DataRow("Action register = () => {}; Action<System.CommandLine.ParseResult> action = _ => register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return action;", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<System.CommandLine.ParseResult> action = _ => register(); register = () => {}; return action;", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<System.CommandLine.ParseResult> action = _ => register(); register = () => {}; return action;", false, false)]
    public async Task Preserves_Captures_In_Returned_Actions(string factoryBody, bool registered, bool localFactory)
    {
        var factory = $$"""
            public static class Factory
            {
                public static Action<System.CommandLine.ParseResult> CreateAction()
                {
                    {{factoryBody}}
                }
            }
            """;
        if (localFactory)
        {
            factory = $$"""
                static Action<System.CommandLine.ParseResult> CreateAction()
                {
                    {{factoryBody}}
                }
                """;
        }

        await VerifyAsync(Source($$"""
            command.SetAction({{(localFactory ? "CreateAction()" : "Factory.CreateAction()")}});
            return command.Parse(args).Invoke();

            {{factory}}
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    private static string Source(string statements, bool registered = true) => $$"""
        {{TestSourceConstants.StandardUsingsWithExtensions}}

        var command = new System.CommandLine.RootCommand();
        {{statements}}

        public class {{(registered ? "BuildModule" : "{|#0:BuildModule|}")}} : Module<List<string>>
        {
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
        }
        """;

    private static DiagnosticResult UnregisteredModule() =>
        VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0).WithArguments("BuildModule");

    private static async Task VerifyAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new VerifyCS.Test
        {
            TestCode = source,
            ReferenceAssemblies = Net.Net110,
        };
        test.TestState.OutputKind = OutputKind.ConsoleApplication;
        test.TestState.AdditionalReferences.Add(typeof(IModuleContext).Assembly.Location);
        test.TestState.AdditionalReferences.Add(typeof(System.CommandLine.RootCommand).Assembly.Location);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}
