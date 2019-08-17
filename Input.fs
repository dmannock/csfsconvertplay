module Input
open System
open System.Collections.Generic
open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax

type UsingCollector() =
    inherit CSharpSyntaxWalker()
    let usings = List<UsingDirectiveSyntax>()
    member public this.Usings = List<UsingDirectiveSyntax>()
    override this.VisitUsingDirective(node) =
        this.Usings.Add(node) |> ignore
        usings.Add(node) |> ignore

let parse (str: string) =
    let tree = CSharpSyntaxTree.ParseText(str, null, "", null)
    let root = tree.GetCompilationUnitRoot() //:?> CompilationUnitSyntax
    let collector = UsingCollector()
    collector.Visit(root)
    let output = String.Join('\n', collector.Usings)
    output