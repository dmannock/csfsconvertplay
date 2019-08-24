[<Literal>]
let ExProgram = """using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
 
namespace TopLevel
{
    using Microsoft;
    using System.ComponentModel;
 
    namespace Child1
    {
        using Microsoft.Win32;
        using System.Runtime.InteropServices;
 
        class Foo {
            public string Hi() {
                return "Hi!";
            }
        }
    }
 
    namespace Child2
    {
        using System.CodeDom;
        using Microsoft.CSharp;
 
        class Bar { }
    }
}"""

[<EntryPoint>]
let main argv =
    // 1. c# parsing -> ast
    //maybe better root option when casted to CSharpSyntaxNode
    let root = (Input.parse ExProgram) 
    // 2. Convereter c# ast -> (intermediate) -> f# ast
    // quick play about in converter.fs, not ready to be hooked up
    // 3. f# ast -> fsharp code
    // namespace with class, method & return type
    let fsharpAst = Output.ExFsharpAst
    // enum ast example
    // let fsharpAst = Output.ExFsharpEnumAst
    printf "ast output to f#:\n%s" (Output.astTest fsharpAst)
    0
