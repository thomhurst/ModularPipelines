using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CallbackCaptureAnalyzerTests
{
    [TestMethod]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); async Task Fail() { await Task.CompletedTask; throw new Exception(); } _ = Fail();", true)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); async Task Fail() { register = () => {}; await Task.CompletedTask; throw new Exception(); } _ = Fail();", false)]
    [DataRow("async Task Fail() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); await Task.CompletedTask; throw new Exception(); } _ = Fail();", true)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); async Task Fail() { try { await Task.CompletedTask; throw new Exception(); } finally { register = () => {}; } } _ = Fail();", false)]
    [DataRow("async Task Fail() { await Task.CompletedTask; throw new Exception(); } _ = Fail(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", true)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); async Task Set() { await Task.Delay(1); register = () => {}; } _ = Set();", true)]
    [DataRow("async Task Set() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); await Task.Delay(1); register = () => {}; } _ = Set();", true)]
    public async Task Preserves_Caller_Continuation_After_Async_Calls(string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("void Set() { replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; } Set(); replace();", true)]
    [DataRow("Action set = () => { replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; }; set(); replace();", true)]
    [DataRow("void Set() { replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; } void Forward() { Set(); replace(); } Forward();", true)]
    [DataRow("void Set() { replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; } Set(); replace = () => {}; replace();", false)]
    [DataRow("void Set() { replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; } if (args.Length > 0) Set(); replace();", true)]
    [DataRow("void Set() { replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; } Set(); Action copy = replace; copy();", true)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); replace = () => { register = () => {}; }; void Set() { replace = () => {}; } Set(); replace();", true)]
    public async Task Resolves_Delegate_Receivers_After_Executed_Captured_Writes(string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                Action replace = () => {};
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", "(register = () => {}) != null", true)]
    [DataRow("register = () => {};", "(register = () => Pipeline.CreateBuilder().AddModule<BuildModule>()) != null", false)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", "(register = () => {}) == null", true)]
    [DataRow("register = () => {};", "(register = () => Pipeline.CreateBuilder().AddModule<BuildModule>()) == null", false)]
    public async Task Searches_Caller_Filters_Before_Callee_Finally(string cleanup, string filter, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                void Fail() { try { throw new Exception(); } finally { {{cleanup}} } }
                try { Fail(); }
                catch (Exception) when ({{filter}}) { }
                catch (Exception) { }
                register!();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Executes_Filter_Callback_Before_Callee_Clears_It()
    {
        await VerifyAsync(Source("""
            callback = _ =>
            {
                Func<bool> register = () => { Pipeline.CreateBuilder().AddModule<BuildModule>(); return true; };
                void Fail() { try { throw new Exception(); } finally { register = () => false; } }
                try { Fail(); }
                catch (Exception) when (register()) { }
            };
            callback(null!); return 0;
            """));
    }

    [TestMethod]
    [DataRow("void Middle() { Fail(); }", "Middle();", "(register = () => {}) != null", true)]
    [DataRow("void Middle() { try { Fail(); } finally { register = () => {}; } }", "Middle();", "(register = () => {}) != null", false)]
    [DataRow("Action fail = Fail;", "fail();", "(register = () => {}) != null", true)]
    [DataRow("", "Fail();", "Reject()", true)]
    [DataRow("", "Fail();", "ThrowFilter()", true)]
    public async Task Preserves_Cross_Call_Unwinding_After_Filter_Search(string forwarding, string call, string filter, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                void Fail() { try { throw new Exception(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } }
                bool Reject() { register = () => {}; return false; }
                bool ThrowFilter() { register = () => {}; throw new Exception(); }
                {{forwarding}}
                try { {{call}} }
                catch (Exception) when ({{filter}}) { }
                catch (Exception) { }
                register!();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", "", true)]
    [DataRow("register = () => {};", "register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", true)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", "register = () => {};", false)]
    [DataRow("register = () => {}; throw new OverflowException();", "register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", true)]
    public async Task Runs_Callee_Cleanup_Before_Selected_Handler(string cleanup, string handler, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                void Fail() { try { throw new Exception(); } finally { {{cleanup}} } }
                try { Fail(); }
                catch (Exception) when ((register = () => {}) != null) { {{handler}} }
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; void Forward() => replace(); Forward();", true)]
    [DataRow("Action replace = () => {}; void Forward() => replace(); replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; Forward();", true)]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; void Forward() => replace(); replace = () => {}; Forward();", false)]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; void Forward() => replace(); void Outer() => Forward(); Outer();", true)]
    [DataRow("register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); void Replace() { register = () => {}; } Replace();", false)]
    [DataRow("void Replace() { throw new Exception(); } Replace(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", false)]
    [DataRow("void Replace() { object? target = null; Func<string?> method = target!.ToString; } try { Replace(); } catch (ArgumentException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", false)]
    [DataRow("void Replace() { object? target = null; Func<string?> method = target!.ToString; } try { Replace(); } catch (NullReferenceException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); register = () => {}; } Replace();", false)]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); register = () => {}; }; replace();", false)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); if (args.Length > 0) register = () => {}; } Replace();", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); if (args.Length > 0) register = () => {}; else register = () => {}; } Replace();", false)]
    [DataRow("void Replace() { try { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return; } finally { register = () => {}; } } Replace();", false)]
    [DataRow("void Replace() { try { register = () => {}; return; } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } } Replace();", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); register = () => {}; throw new Exception(); } try { Replace(); } catch (Exception) {}", false)]
    [DataRow("void Replace() { try { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); throw new Exception(); } finally { register = () => {}; } } try { Replace(); } catch (Exception) {}", false)]
    [DataRow("void Replace() { try { throw new Exception(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } } try { Replace(); } catch (Exception) {}", true)]
    public async Task Transfers_Captured_Delegate_Writes_In_Execution_Order(string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("(register, _) = (register, 0);", true)]
    [DataRow("(_, register) = (0, register);", true)]
    [DataRow("((register, _), _) = ((register, 0), 1);", true)]
    [DataRow("(register, _) = (register, register = () => {});", true)]
    [DataRow("(register, _) = (register = () => {}, register);", false)]
    [DataRow("Action copy = register; (register, _) = (copy, 0);", true)]
    [DataRow("(register, _) = (register, 0); (register, _) = (register, 1);", true)]
    public async Task Resolves_Tuple_Copies_At_Their_Read_Sites(string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } Replace();", true)]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; replace();", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", false)]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); };", false)]
    [DataRow("void Replace() { register = () => {}; } Replace();", false)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } Replace(); register = () => {};", false)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } if (args.Length > 0) Replace();", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } Action replace = Replace; replace();", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } void Forward() { Replace(); } Forward();", true)]
    [DataRow("Action replace = () => { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }; replace = () => {}; replace();", false)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); if (args.Length > 0) Replace(); } Replace();", true)]
    [DataRow("void Replace() { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); throw new Exception(); } try { Replace(); } catch (Exception) {}", true)]
    public async Task Tracks_Captured_Delegate_Writes_From_Executed_Callables(string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }
    [TestMethod]
    [DataRow("default(T) as IDisposable", true)]
    [DataRow("value as IDisposable", true)]
    [DataRow("(object)value as IDisposable", true)]
    [DataRow("value as object", false)]
    public async Task Preserves_Fallible_Type_Parameter_TryCasts(string expression, bool registered)
    {
        await VerifyAsync(Source($$"""
            Execute<int>(0); return 0;
            static void Execute<T>(T value) where T : struct
            {
                Action callback = () =>
                {
                    var target = {{expression}};
                    Action register = () => {};
                    try { Func<string?> method = target!.ToString; }
                    catch (NullReferenceException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                    register();
                };
                callback();
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("(register, _) = ((Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()), 0);", true)]
    [DataRow("(_, register) = (0, (Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()));", true)]
    [DataRow("((_, register), _) = ((0, (Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>())), 1);", true)]
    [DataRow("(register, _) = ((Action)(() => {}), 0);", false)]
    [DataRow("(register, _) = ((Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()), 0); register = () => {};", false)]
    [DataRow("(register, register) = ((Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()), (Action)(() => {}));", false)]
    [DataRow("(register, register) = ((Action)(() => {}), (Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()));", true)]
    [DataRow("(register, _) = ((Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()), register = () => {});", true)]
    [DataRow("(register, _) = ((Action)(() => {}), register = () => Pipeline.CreateBuilder().AddModule<BuildModule>());", false)]
    [DataRow("if (args.Length > 0) (register, _) = ((Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()), 0);", true)]
    public async Task Tracks_Deconstructed_Delegate_Definitions(string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Tracks_Deconstructed_Delegate_Declaration()
    {
        await VerifyAsync(Source("""
            callback = _ =>
            {
                var (register, number) = ((Action)(() => Pipeline.CreateBuilder().AddModule<BuildModule>()), 0);
                register();
            };
            callback(null!); return 0;
            """));
    }

    [TestMethod]
    [DataRow("Action register = () => {};", "register += () => Pipeline.CreateBuilder().AddModule<BuildModule>();", true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", "register += () => {};", true)]
    [DataRow("Action register = () => {};", "register += () => {};", false)]
    [DataRow("Action register = () => {};", "register += () => Pipeline.CreateBuilder().AddModule<BuildModule>(); register = () => {};", false)]
    [DataRow("Action register = () => {};", "if (args.Length > 0) register += () => Pipeline.CreateBuilder().AddModule<BuildModule>();", true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();", "register += (register = () => {});", true)]
    [DataRow("Action register = () => {};", "register += (register = () => Pipeline.CreateBuilder().AddModule<BuildModule>());", true)]
    public async Task Tracks_Delegate_Additions(string declaration, string mutation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                {{declaration}}
                {{mutation}}
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("new Counter()", false)]
    [DataRow("default(Counter)", false)]
    [DataRow("(Counter?)null", true)]
    [DataRow("new Counter?(new Counter())", true)]
    public async Task Preserves_NonNull_Boxing_Proofs(string expression, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                object? target = {{expression}};
                Action register = () => {};
                try { Func<string?> method = target!.ToString; }
                catch (NullReferenceException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public struct Counter { }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("Exception", true)]
    [DataRow("InvalidOperationException", true)]
    [DataRow("Exception", false)]
    public async Task Tracks_Type_Parameter_Constructor_Exceptions(string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            Execute<Thrower>(); return 0;
            static void Execute<T>() where T : new()
            {
                Action callback = () =>
                {
                    Action register = () => {};
                    try { _ = new T(); }
                    catch ({{caught}}) { register = () => {{(registered ? "Pipeline.CreateBuilder().AddModule<BuildModule>()" : "{}")}}; }
                    register();
                };
                callback();
            }

            public class Thrower
            {
                public Thrower() => throw new InvalidOperationException();
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = Holder.Value;", "TypeInitializationException", true)]
    [DataRow("Holder.Value = 1;", "TypeInitializationException", true)]
    [DataRow("Holder.Value += 1;", "TypeInitializationException", true)]
    [DataRow("Holder.Value++;", "TypeInitializationException", true)]
    [DataRow("ref int value = ref Holder.Value;", "TypeInitializationException", true)]
    [DataRow("_ = Holder.Value;", "Exception", true)]
    [DataRow("_ = Holder.Value;", "InvalidOperationException", false)]
    [DataRow("_ = Holder.Constant;", "TypeInitializationException", false)]
    public async Task Tracks_Static_Field_Initialization_Exception_Paths(string operation, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try { {{operation}} }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public static class Holder
            {
                public static int Value;
                public const int Constant = 1;
                static Holder() => throw new InvalidOperationException();
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Static_Field_Initialization_Follows_Assigned_Value(bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try
                {
                    Holder.Value = register = () => {{(registered ? "Pipeline.CreateBuilder().AddModule<BuildModule>()" : "{}")}};
                }
                catch (TypeInitializationException) { register(); }
            };
            callback(null!); return 0;

            public static class Holder
            {
                public static Action Value = null!;
                static Holder() => throw new InvalidOperationException();
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("throw new CustomException();", "InvalidOperationException")]
    [DataRow("throw new CustomException();", "CustomException")]
    [DataRow("_ = true ? throw new CustomException() : 0;", "InvalidOperationException")]
    public async Task Tracks_Thrown_Exception_Constructor_Failures(string operation, string caught)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try { {{operation}} }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public class CustomException : Exception
            {
                public CustomException() => throw new InvalidOperationException();
            }
            """));
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Exception_Construction_Follows_Argument_Evaluation(bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try
                {
                    throw new CustomException(register = () => {{(registered ? "Pipeline.CreateBuilder().AddModule<BuildModule>()" : "{}")}});
                }
                catch (InvalidOperationException) { register(); }
            };
            callback(null!); return 0;

            public class CustomException : Exception
            {
                public CustomException(Action value) => throw new InvalidOperationException();
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("int number = argument;", true)]
    [DataRow("int? number = argument;", true)]
    [DataRow("string text = argument;", true)]
    [DataRow("IDisposable disposable = argument;", true)]
    [DataRow("int number = (int)argument;", true)]
    [DataRow("object value = argument;", false)]
    [DataRow("dynamic value = argument;", false)]
    [DataRow("object value = (object)argument;", false)]
    [DataRow("string? text = argument as string;", false)]
    [DataRow("int? number = argument as int?;", false)]
    public async Task Tracks_Dynamic_Conversion_Exception_Paths(string operation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                dynamic argument = new object();
                Action register = () => {};
                try { {{operation}} }
                catch (Exception) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Preserves_Capture_After_Dynamic_Conversion_Operand(bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try
                {
                    int number = (dynamic)(register = () => {{(registered ? "Pipeline.CreateBuilder().AddModule<BuildModule>()" : "{}")}});
                    register = () => {};
                }
                catch (Exception) { register(); }
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    public async Task Tracks_Dynamic_User_Defined_Conversion_Exception_Path()
    {
        await VerifyAsync(Source("""
            callback = _ =>
            {
                dynamic argument = default(Convertible);
                Action register = () => {};
                try { int number = argument; }
                catch (InvalidOperationException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public struct Convertible
            {
                public static implicit operator int(Convertible value) => throw new InvalidOperationException();
            }
            """));
    }

    [TestMethod]
    [DataRow("new Thrower(argument);")]
    [DataRow("_ = new Thrower(argument);")]
    [DataRow("new Thrower(argument = register = () => Pipeline.CreateBuilder().AddModule<BuildModule>());", true)]
    [DataRow("new Thrower(argument = register = () => {});", false)]
    public async Task Tracks_Dynamic_Constructor_Exception_Paths(string operation, bool registered = true)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                dynamic argument = 1;
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try
                {
                    {{operation}}
                    register = () => {};
                }
                catch (InvalidOperationException) { register(); }
            };
            callback(null!); return 0;

            public class Thrower
            {
                public Thrower(object value) => throw new InvalidOperationException();
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("_ = left / right;")]
    [DataRow("_ = 1 / right;")]
    [DataRow("left /= right;")]
    [DataRow("int value = 1; value /= right;")]
    [DataRow("left += right;")]
    [DataRow("left++;")]
    [DataRow("--left;")]
    [DataRow("_ = -left;")]
    [DataRow("_ = checked(-left);")]
    public async Task Tracks_Dynamic_Operator_Exception_Paths(string operation)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                dynamic left = default(Counter);
                dynamic right = default(Counter);
                Action register = () => {};
                try { {{operation}} }
                catch (InvalidOperationException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public struct Counter
            {
                public static Counter operator /(Counter left, Counter right) => throw new InvalidOperationException();
                public static Counter operator /(int left, Counter right) => throw new InvalidOperationException();
                public static Counter operator +(Counter left, Counter right) => throw new InvalidOperationException();
                public static Counter operator ++(Counter value) => throw new InvalidOperationException();
                public static Counter operator --(Counter value) => throw new InvalidOperationException();
                public static Counter operator -(Counter value) => throw new InvalidOperationException();
            }
            """));
    }

    [TestMethod]
    [DataRow("object target = null!;", "target.ToString", "NullReferenceException", true)]
    [DataRow("object target = null!;", "new Func<string?>(target.ToString)", "NullReferenceException", true)]
    [DataRow("object target = null!;", "target.ToString", "ArgumentException", false)]
    [DataRow("var target = new object();", "target.ToString", "NullReferenceException", false)]
    [DataRow("", "MethodGroups.StaticText", "NullReferenceException", false)]
    [DataRow("object target = null!;", "target.ExtensionText", "NullReferenceException", false)]
    [DataRow("int target = 0;", "target.ToString", "NullReferenceException", false)]
    [DataRow("int? target = null;", "target.ToString", "NullReferenceException", false)]
    [DataRow("object target = null!;", "() => target.ToString()", "NullReferenceException", false)]
    [DataRow("object target = null!; string? Text() => target.ToString();", "Text", "NullReferenceException", false)]
    [DataRow("var target = new object(); Clear(ref target);", "target.ToString", "NullReferenceException", true)]
    public async Task Tracks_Method_Group_Exception_Paths(string setup, string value, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                {{setup}}
                Action register = () => {};
                try { Func<string?> text = {{value}}; }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
                static void Clear(ref object value) => value = null!;
            };
            callback(null!); return 0;

            public static class MethodGroups
            {
                public static string StaticText() => "";
                public static string ExtensionText(this object value) => "";
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("object target = null!;", true)]
    [DataRow("var target = new object();", false)]
    public async Task Preserves_Capture_Before_Method_Group_Exception(string setup, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                {{setup}}
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try
                {
                    Func<string?> text = target.ToString;
                    register = () => {};
                }
                catch (NullReferenceException) { }
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("int counter = 1; int divisor = 0;", "counter /= divisor;", "DivideByZeroException", true)]
    [DataRow("int counter = 1; int divisor = 0;", "counter %= divisor;", "DivideByZeroException", true)]
    [DataRow("int counter = int.MaxValue;", "checked { counter += 1; }", "OverflowException", true)]
    [DataRow("int counter = int.MinValue;", "checked { counter -= 1; }", "OverflowException", true)]
    [DataRow("int counter = int.MaxValue;", "checked { counter *= 2; }", "OverflowException", true)]
    [DataRow("int counter = int.MaxValue;", "checked { counter++; }", "OverflowException", true)]
    [DataRow("int counter = int.MaxValue;", "checked { ++counter; }", "OverflowException", true)]
    [DataRow("int counter = int.MinValue;", "checked { counter--; }", "OverflowException", true)]
    [DataRow("int counter = int.MinValue;", "checked { --counter; }", "OverflowException", true)]
    [DataRow("short counter = short.MaxValue;", "checked { counter++; }", "OverflowException", true)]
    [DataRow("int counter = int.MaxValue;", "unchecked { counter++; }", "OverflowException", false)]
    [DataRow("int counter = int.MinValue;", "unchecked { counter--; }", "OverflowException", false)]
    [DataRow("int counter = int.MaxValue;", "unchecked { counter += 1; }", "OverflowException", false)]
    [DataRow("float counter = 1; float divisor = 0;", "counter /= divisor;", "DivideByZeroException", false)]
    [DataRow("double counter = 1; double divisor = 0;", "counter %= divisor;", "DivideByZeroException", false)]
    [DataRow("double counter = double.MaxValue;", "checked { counter++; }", "OverflowException", false)]
    [DataRow("float? counter = 1; float divisor = 0;", "counter /= divisor;", "DivideByZeroException", false)]
    [DataRow("decimal counter = decimal.MaxValue;", "counter += 1;", "OverflowException", true)]
    [DataRow("decimal counter = decimal.MaxValue;", "counter++;", "OverflowException", true)]
    [DataRow("decimal counter = 1; decimal divisor = 0;", "counter /= divisor;", "DivideByZeroException", true)]
    [DataRow("decimal counter = decimal.MaxValue;", "counter = counter + 1;", "OverflowException", true)]
    [DataRow("int counter = int.MaxValue;", "checked { counter++; }", "InvalidOperationException", false)]
    [DataRow("int counter = int.MaxValue;", "checked { counter += 1; }", "ArgumentException", false)]
    [DataRow("int counter = int.MaxValue;", "checked { counter = counter + 1; }", "InvalidOperationException", false)]
    [DataRow("int counter = int.MinValue;", "checked { counter = -counter; }", "InvalidOperationException", false)]
    [DataRow("int counter = int.MinValue;", "checked { counter = -counter; }", "OverflowException", true)]
    [DataRow("int counter = 1; int divisor = 0;", "counter /= divisor;", "InvalidOperationException", false)]
    [DataRow("int counter = int.MinValue; int divisor = -1;", "counter /= divisor;", "OverflowException", true)]
    [DataRow("int counter = int.MinValue; int divisor = -1;", "counter %= divisor;", "OverflowException", true)]
    [DataRow("long counter = long.MinValue; long divisor = -1;", "counter %= divisor;", "OverflowException", true)]
    [DataRow("nint counter = nint.MinValue; nint divisor = -1;", "counter %= divisor;", "OverflowException", true)]
    [DataRow("int? counter = int.MinValue; int divisor = -1;", "counter %= divisor;", "OverflowException", true)]
    [DataRow("short counter = short.MinValue; short divisor = -1;", "checked { counter /= divisor; }", "OverflowException", true)]
    [DataRow("short counter = short.MinValue; short divisor = -1;", "unchecked { counter /= divisor; }", "OverflowException", false)]
    [DataRow("uint counter = 1; uint divisor = 0;", "counter /= divisor;", "OverflowException", false)]
    [DataRow("uint counter = 1; uint divisor = 0;", "checked { counter /= divisor; }", "OverflowException", false)]
    [DataRow("decimal counter = decimal.MaxValue;", "counter *= 2;", "ArgumentException", false)]
    [DataRow("decimal counter = 1; decimal divisor = 0;", "counter %= divisor;", "OverflowException", false)]
    [DataRow("int counter = int.MinValue;", "checked { counter--; }", "ArithmeticException", true)]
    public async Task Tracks_Arithmetic_Assignment_Exception_Paths(string setup, string operation, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                {{setup}}
                Action register = () => {};
                try { {{operation}} }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("counter += counter;")]
    [DataRow("counter++;")]
    [DataRow("--counter;")]
    public async Task Tracks_Overloaded_Assignment_Operator_Exception_Paths(string operation)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Counter counter = default;
                Action register = () => {};
                try { {{operation}} }
                catch (InvalidOperationException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public struct Counter
            {
                public static Counter operator +(Counter left, Counter right) => throw new InvalidOperationException();
                public static Counter operator ++(Counter value) => throw new InvalidOperationException();
                public static Counter operator --(Counter value) => throw new InvalidOperationException();
            }
            """));
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Tracks_Compound_Assignment_Conversion_Exception_Paths(bool inputConversionThrows)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Counter counter = default;
                Action register = () => {};
                try { counter += 1; }
                catch (InvalidOperationException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public struct Counter
            {
                public static implicit operator int(Counter value) => {{(inputConversionThrows ? "throw new InvalidOperationException()" : "0")}};
                public static implicit operator Counter(int value) => {{(inputConversionThrows ? "default" : "throw new InvalidOperationException()")}};
            }
            """));
    }

    [TestMethod]
    [DataRow("Publisher.Callback", "public static Action Callback", false)]
    [DataRow("Publisher.Callback", "public static Action Callback", true)]
    [DataRow("publisher.Callback", "public Action Callback", false)]
    [DataRow("publisher.Callback", "public Action Callback", true)]
    [DataRow("publisher[0]", "public Action this[int index]", false)]
    [DataRow("publisher[0]", "public Action this[int index]", true)]
    public async Task Property_Setter_Exceptions_Follow_Value_Evaluation(string target, string property, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                var publisher = new Publisher();
                Action register = {{(registered ? "() => {}" : "() => Pipeline.CreateBuilder().AddModule<BuildModule>()")}};
                try { {{target}} = register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}}; }
                catch (InvalidOperationException) { register(); }
            };
            callback(null!); return 0;

            public class Publisher
            {
                {{property}}
                {
                    set { throw new InvalidOperationException(); }
                }
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("+=")]
    [DataRow("-=")]
    public async Task Compound_Property_Setter_Exceptions_Follow_Value_Evaluation(string assignment)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try { Publisher.Callback {{assignment}} register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                catch (InvalidOperationException) { register(); }
            };
            callback(null!); return 0;

            public static class Publisher
            {
                public static Action? Callback
                {
                    get => () => {};
                    set { throw new InvalidOperationException(); }
                }
            }
            """));
    }

    [TestMethod]
    [DataRow("Publisher.Callback", "public static ref Action? Callback", "=", false)]
    [DataRow("Publisher.Callback", "public static ref Action? Callback", "=", true)]
    [DataRow("Publisher.Callback", "public static ref Action? Callback", "+=", false)]
    [DataRow("Publisher.Callback", "public static ref Action? Callback", "+=", true)]
    [DataRow("Publisher.Callback", "public static ref Action? Callback", "-=", false)]
    [DataRow("Publisher.Callback", "public static ref Action? Callback", "-=", true)]
    [DataRow("publisher[0]", "public ref Action? this[int index]", "=", false)]
    [DataRow("publisher[0]", "public ref Action? this[int index]", "=", true)]
    [DataRow("publisher[0]", "public ref Action? this[int index]", "+=", false)]
    [DataRow("publisher[0]", "public ref Action? this[int index]", "+=", true)]
    [DataRow("publisher[0]", "public ref Action? this[int index]", "-=", false)]
    [DataRow("publisher[0]", "public ref Action? this[int index]", "-=", true)]
    public async Task Ref_Property_Getter_Exceptions_Precede_Value_Evaluation(string target, string property, string assignment, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                var publisher = new Publisher();
                Action register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                try { {{target}} {{assignment}} register = {{(registered ? "() => {}" : "() => Pipeline.CreateBuilder().AddModule<BuildModule>()")}}; }
                catch (InvalidOperationException) { register(); }
            };
            callback(null!); return 0;

            public class Publisher
            {
                {{property}}
                {
                    get { throw new InvalidOperationException(); }
                }
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("+=")]
    [DataRow("-=")]
    public async Task Tracks_Event_Accessor_Exception_Paths_For_Callback_Definitions(string assignment)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => {};
                try { Publisher.Event {{assignment}} () => {}; }
                catch (InvalidOperationException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public static class Publisher
            {
                public static event Action Event
                {
                    add { throw new InvalidOperationException(); }
                    remove { throw new InvalidOperationException(); }
                }
            }
            """));
    }

    [TestMethod]
    [DataRow("+=")]
    [DataRow("-=")]
    public async Task Event_Accessor_Exceptions_Follow_Handler_Evaluation(string assignment)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try { Publisher.Event {{assignment}} register = () => {}; }
                catch (InvalidOperationException) { }
                register();
            };
            callback(null!); return 0;

            public static class Publisher
            {
                public static event Action Event
                {
                    add { throw new InvalidOperationException(); }
                    remove { throw new InvalidOperationException(); }
                }
            }
            """, registered: false), UnregisteredModule());
    }

    [TestMethod]
    [DataRow("new Holder()", "NullReferenceException", false)]
    [DataRow("null!", "InvalidOperationException", false)]
    [DataRow("null!", "NullReferenceException", true)]
    public async Task Tracks_Field_Exception_Paths_For_Callback_Definitions(string receiver, string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                Holder holder = {{receiver}};
                Action register = () => {};
                try { _ = holder.Value; }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public class Holder { public int Value; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("new object() as Holder", "_ = holder.Value;", true)]
    [DataRow("new object() as Holder", "holder.Value = 1;", true)]
    [DataRow("new object() as Holder", "holder.Value += 1;", true)]
    [DataRow("new object() as Holder", "holder.Value++;", true)]
    [DataRow("new object() as Holder", "Func<string?> text = holder.ToString;", true)]
    [DataRow("(Holder)(new object() as Holder)!", "_ = holder.Value;", true)]
    [DataRow("(dynamic)new NullHolder()", "_ = holder.Value;", true)]
    [DataRow("new NullHolder()", "_ = holder.Value;", true)]
    [DataRow("(Holder)(object)new Holder()", "_ = holder.Value;", false)]
    [DataRow("(Holder)(object)new Holder()", "Func<string?> text = holder.ToString;", false)]
    [DataRow("new Holder() as Holder", "_ = holder.Value;", false)]
    public async Task Non_Null_Receiver_Proof_Respects_Conversions(string receiver, string operation, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = _ =>
            {
                Holder original = ({{receiver}})!;
                Holder holder = original;
                Action register = () => {};
                try { {{operation}} }
                catch (NullReferenceException) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;

            public class Holder { public int Value; }
            public class NullHolder
            {
                public static implicit operator Holder(NullHolder value) => null!;
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("InvalidOperationException", false)]
    [DataRow("IndexOutOfRangeException", true)]
    [DataRow("NullReferenceException", true)]
    [DataRow("Exception", true)]
    public async Task Matches_Array_Exception_Type_For_Callback_Definitions(string caught, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                Action register = () => {};
                try { _ = args[0]; }
                catch ({{caught}}) { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }
                register();
            };
            callback(null!); return 0;
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("ref", "object", "args", true)]
    [DataRow("out", "object", "args", true)]
    [DataRow("in", "object", "args", false)]
    [DataRow("", "object", "args", false)]
    [DataRow("ref", "string", "args", false)]
    [DataRow("out", "string", "args", false)]
    [DataRow("ref", "int", "new int[1]", false)]
    [DataRow("out", "int", "new int[1]", false)]
    [DataRow("ref", "object", "new object[1]", false)]
    [DataRow("out", "object", "new object[1]", false)]
    public async Task Array_Argument_Checks_Precede_Later_Argument_Evaluation(
        string modifier, string elementType, string values, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                {{elementType}}[] values = {{values}};
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try
                {
                    Touch({{modifier}} values[0], register = () => {});
                    register = () => {};
                }
                catch (ArrayTypeMismatchException) { }
                register();
            };
            callback(null!); return 0;

            void Touch({{modifier}} {{elementType}} value, Action replacement)
            {
                {{(modifier == "out" ? "value = default!;" : string.Empty)}}
            }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("object[] original = new object[1]; object[] values = original;", false)]
    [DataRow("object[] values = args; values = new object[1];", false)]
    [DataRow("object[] values = new object[1]; values = args;", true)]
    [DataRow("object[] values; if (args.Length > 0) values = new object[1]; else values = new object[2];", false)]
    [DataRow("object[] values = new object[1]; if (args.Length > 0) values = args;", true)]
    [DataRow("object[] original = args; object[] values = original;", true)]
    [DataRow("object[] values = new object[1]; Replace(ref values);", true)]
    [DataRow("object[] values = new object[1]; ref object[] alias = ref values; alias = args;", true)]
    [DataRow("object[] values = new object[1]; object[] other = new object[1]; ref object[] alias = ref other; alias = ref values; alias = args;", true)]
    [DataRow("object[] values = new object[1]; Action replace = () => values = args; replace();", true)]
    [DataRow("object[] values = new object[1]; (values, _) = (args, 0);", true)]
    public async Task Tracks_Runtime_Array_Types_At_Writable_Arguments(string declarations, bool registered)
    {
        await VerifyAsync(Source($$"""
            callback = parseResult =>
            {
                {{declarations}}
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                try
                {
                    Touch(ref values[0], register = () => {});
                    register = () => {};
                }
                catch (ArrayTypeMismatchException) { }
                register();
            };
            callback(null!); return 0;

            void Touch(ref object value, Action replacement) { }
            void Replace(ref object[] array) { array = args; }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            callback = _ =>
            {
                var copy = register;
                {{(invoked ? "copy();" : string.Empty)}}
            };
            callback(null!); return 0;
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Tracks_Delegate_Member_Copied_Inside_Action(bool invoked)
    {
        var source = Source($$"""
            callback = _ =>
            {
                var copy = Callbacks.Register;
                {{(invoked ? "copy();" : string.Empty)}}
            };
            callback(null!); return 0;
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
            callback = _ =>
            {
                Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                {{(invoked ? "register();" : string.Empty)}}
            };
            callback(null!); return 0;
            """, registered: invoked);

        await VerifyAsync(source, invoked ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow(true, false)]
    [DataRow(false, true)]
    public async Task Tracks_Captured_Delegate_Reassigned_After_Action_Installation(bool initiallyRegistered, bool registered)
    {
        var source = Source($$"""
            Action register = {{(initiallyRegistered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            callback = _ => register();
            register = {{(registered ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
            callback(null!); return 0;
            """, registered);

        await VerifyAsync(source, registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("callback = _ => { var copy = register; copy(); };")]
    [DataRow("callback = _ => { Action inner = () => register(); inner(); };")]
    [DataRow("callback = _ => { void Inner() => register(); Inner(); };")]
    public async Task Tracks_Reassigned_Delegate_Through_Nested_Callbacks(string install)
    {
        var source = Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            {{install}}
            register = () => {};
            callback(null!); return 0;
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
            callback = _ => register();
            callback(null!);
            register = () => {};
            callback(null!); return 0;
            """));
    }

    [TestMethod]
    public async Task Preserves_Delegate_Value_Copied_Before_Reassignment()
    {
        await VerifyAsync(Source("""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            var copy = register;
            callback = _ => copy();
            register = () => {};
            callback(null!); return 0;
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
            callback = _ =>
            {
                if (args.Length > 0)
                    register = {{(firstRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                else
                    register = {{(secondRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                register();
            };
            callback(null!); return 0;
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
            callback = _ =>
            {
                if (args.Length > 0)
                    register = {{(replacementRegisters ? "() => Pipeline.CreateBuilder().AddModule<BuildModule>()" : "() => {}")}};
                register();
            };
            callback(null!); return 0;
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
            callback = _ =>
            {
                if (args.Length > 0)
                {
                    register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    {{exit}}
                }
                Invoke:
                register();
                End:;
            };
            callback(null!); return 0;
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
            callback = _ =>
            {
                {{statements}}
            };
            callback(null!); return 0;
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
            callback = _ =>
            {
                {{replacement}}
                register();
            };
            callback(null!); return 0;
            """, registered: false);

        await VerifyAsync(source, UnregisteredModule());
    }

    [TestMethod]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return _ => register();", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return _ => register();", true, false)]
    [DataRow("Action register = () => {}; return _ => register();", false, true)]
    [DataRow("Action register = () => {}; return _ => register();", false, false)]
    [DataRow("Action register = () => {}; Action<object> action = _ => register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return action;", true, true)]
    [DataRow("Action register = () => {}; Action<object> action = _ => register(); register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); return action;", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> action = _ => register(); register = () => {}; return action;", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> action = _ => register(); register = () => {}; return action;", false, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { register = () => {}; }", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { register = () => {}; }", false, false)]
    [DataRow("Action register = () => {}; try { return _ => register(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, true)]
    [DataRow("Action register = () => {}; try { return _ => register(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> action = _ => register(); try { return action; } finally { register = () => {}; }", false, true)]
    [DataRow("Action register = () => {}; Action<object> action = _ => register(); try { return action; } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { try { return _ => register(); } finally { register = () => {}; } } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { try { return _ => register(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); } } finally { register = () => {}; }", false, false)]
    [DataRow("Action register = () => {}; try { return _ => register(); } finally { if (Environment.TickCount > 0) register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { if (Environment.TickCount > 0) register = () => {}; }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { try { return _ => register(); } finally { throw new InvalidOperationException(); } } catch (InvalidOperationException) { return _ => {}; }", false, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); try { return _ => register(); } finally { try { throw new InvalidOperationException(); } catch (InvalidOperationException) { register = () => {}; } }", false, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> Inner() => _ => register(); return Inner();", true, true)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> Inner() => _ => register(); return Inner();", true, false)]
    [DataRow("Action register = () => {}; Action<object> Inner() => _ => register(); return Inner();", false, true)]
    [DataRow("Action register = () => {}; Action<object> Inner() => _ => register(); return Inner();", false, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> Inner() => _ => register(); try { return Inner(); } finally { register = () => {}; }", false, true)]
    [DataRow("Action register = () => {}; Action<object> Inner() => _ => register(); try { return Inner(); } finally { register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); }", true, false)]
    [DataRow("Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>(); Action<object> Inner() => _ => register(); Action<object> Forward() => Inner(); return Forward();", true, true)]
    [DataRow("Action register = () => {}; Action<object> Inner() => _ => register(); Action<object> Forward() => Inner(); return Forward();", false, false)]
    public async Task Preserves_Captures_In_Returned_Actions(string factoryBody, bool registered, bool localFactory)
    {
        var factory = $$"""
            public static class Factory
            {
                public static Action<object> CreateAction()
                {
                    {{factoryBody}}
                }
            }
            """;
        if (localFactory)
        {
            factory = $$"""
                static Action<object> CreateAction()
                {
                    {{factoryBody}}
                }
                """;
        }

        await VerifyAsync(Source($$"""
            callback = {{(localFactory ? "CreateAction()" : "Factory.CreateAction()")}};
            callback(null!); return 0;

            {{factory}}
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("NullReferenceException", "", false)]
    [DataRow("OverflowException", "", true)]
    [DataRow("Exception", "", true)]
    [DataRow("NullReferenceException", "try { object target = null!; Func<string?> method = target.ToString; } catch (NullReferenceException) { }", false)]
    [DataRow("OverflowException", "try { object target = null!; Func<string?> method = target.ToString; } catch (NullReferenceException) { }", true)]
    [DataRow("NullReferenceException", "try { throw; } catch (OverflowException) { throw; }", false)]
    [DataRow("OverflowException", "try { throw; } catch (OverflowException) { throw; }", true)]
    public async Task Preserves_Active_Exception_Type_Across_Rethrows(string caught, string nestedHandler, bool registered)
    {
        await VerifyAsync(Source($$"""
            Action register = () => {};
            try
            {
                try
                {
                    register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    int maximum = int.MaxValue;
                    checked { maximum++; }
                }
                catch (OverflowException)
                {
                    {{nestedHandler}}
                    throw;
                }
            }
            catch ({{caught}}) { register(); }
            """, registered), registered ? [] : [UnregisteredModule()]);
    }

    [TestMethod]
    [DataRow("ref Holder alias = ref (args.Length > 0 ? ref holder : ref other); alias = null!;")]
    [DataRow("ref Holder alias = ref (args.Length > 0 ? ref other : ref holder); alias = null!;")]
    [DataRow("ref Holder alias = ref other; alias = ref (args.Length > 0 ? ref holder : ref other); alias = null!;")]
    [DataRow("ref Holder alias = ref other; alias = ref (args.Length > 0 ? ref other : ref holder); alias = null!;")]
    [DataRow("Clear(ref (args.Length > 0 ? ref holder : ref other));")]
    [DataRow("Clear(ref (args.Length > 0 ? ref other : ref holder));")]
    public async Task Conditional_Ref_Aliases_Invalidate_NonNull_Receiver_Proof(string mutation)
    {
        await VerifyAsync(Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            #nullable disable
            var holder = new Holder();
            var other = new Holder();
            {{mutation}}
            try
            {
                _ = holder.Value;
                register = () => {};
            }
            catch (NullReferenceException) { }
            register();
            static void Clear(ref Holder value) => value = null!;

            public class Holder { public int Value; }
            """));
    }

    [TestMethod]
    [DataRow("ref object[] alias = ref (args.Length > 0 ? ref values : ref other); alias = new string[1];")]
    [DataRow("ref object[] alias = ref (args.Length > 0 ? ref other : ref values); alias = new string[1];")]
    [DataRow("ref object[] alias = ref other; alias = ref (args.Length > 0 ? ref values : ref other); alias = new string[1];")]
    [DataRow("ref object[] alias = ref other; alias = ref (args.Length > 0 ? ref other : ref values); alias = new string[1];")]
    [DataRow("Replace(ref (args.Length > 0 ? ref values : ref other));")]
    [DataRow("Replace(ref (args.Length > 0 ? ref other : ref values));")]
    public async Task Conditional_Ref_Aliases_Invalidate_Exact_Array_Type_Proof(string mutation)
    {
        await VerifyAsync(Source($$"""
            Action register = () => Pipeline.CreateBuilder().AddModule<BuildModule>();
            object[] values = new object[1];
            object[] other = new object[1];
            var item = new object();
            {{mutation}}
            try
            {
                values[0] = item;
                register = () => {};
            }
            catch (ArrayTypeMismatchException) { }
            register();
            static void Replace(ref object[] value) => value = new string[1];
            """));
    }

    private static string Source(string statements, bool registered = true) => $$"""
        {{TestSourceConstants.StandardUsingsWithExtensions}}

        Action<object> callback = _ => {};
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
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}
