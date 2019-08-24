module Converter
open System
open System.Collections.Generic
open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax

// TODO: testing what parsing / symbol / types are available
module ColHelpers =
    let map f (arr:Collections.Immutable.ImmutableArray<_>) =
        [| for i = 0 to arr.Length - 1 do yield f(arr.Item(i)) |]

let namedTypeMatchTest (t: INamedTypeSymbol) =
    match t.TypeKind with
    | TypeKind.Class
    | TypeKind.Struct
    | TypeKind.Interface
    | TypeKind.Enum
    | TypeKind.Delegate
    | _ -> failwithf "unsupported type %s" (t.TypeKind.ToString())

let rec namespaceTest (ns: INamespaceSymbol) =
    let members = ns.GetTypeMembers()
    members 
    |> ColHelpers.map namedTypeMatchTest 
    |> ignore //do sumething here
    // let subNs = ns.GetNamespaceMembers()
    // subNs 
    // |> ColHelpers.map nameSpaceTest
    // |> ignore

let matchMember: MemberDeclarationSyntax -> string =
    fun (mem: MemberDeclarationSyntax) ->
        match mem with
        | :? NamespaceDeclarationSyntax as ns -> ns.Name.GetText().ToString()
        | _ -> ""

let loopSyntaxList (sList: SyntaxList<MemberDeclarationSyntax>) predicate = 
    let mutable arr = [||]
    for i = 0 to sList.Count - 1 do 
        let value = sList.Item(i)
        arr.[i] <- (predicate value)
    arr    

let walk (root: CompilationUnitSyntax) =
    let rec loop (node: CompilationUnitSyntax) str =
        if node.Members.Count = 0
        then str
        else
            let names = loopSyntaxList node.Members matchMember
            // let names = node.Members |> List.map matchMember
            let joined = String.Join(',', names)
            let newStr = str + joined
            loop node newStr
    loop root ""
