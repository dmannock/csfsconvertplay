﻿// Learn more about F# at http://fsharp.org

open System

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
 
        class Foo { }
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
    printf "output is:\n%s" (Input.parse ExProgram)
    0 // return an integer exit code