module Input
open System
open System.Collections.Generic
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax

type UsingCollector() =
    inherit CSharpSyntaxWalker()
    member val Usings = List<UsingDirectiveSyntax>() with get, set
    member val Namespaces = List<NamespaceDeclarationSyntax>() with get, set
    override this.VisitUsingDirective(node) =
        this.Usings.Add(node) |> ignore
    override this.VisitNamespaceDeclaration(node) =
        this.Namespaces.Add(node) |> ignore

let parse (str: string) =
    let tree = CSharpSyntaxTree.ParseText(str, null, "", null)
    let root = tree.GetCompilationUnitRoot()
    let collector = UsingCollector()
    collector.Visit(root)
    let usings = String.Join('\n', collector.Usings)
    let namespaces = String.Join('\n', collector.Namespaces)
    let output = sprintf "Usings:%s\nNamespaces:%s\n" usings namespaces
    output