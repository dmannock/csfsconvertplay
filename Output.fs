module Output
open Microsoft.FSharp.Compiler.Ast
open Microsoft.FSharp.Compiler.Range
open Fantomas

[<Literal>]
let ExFsharp = """module TopLevel
type Foo() = 
    member x.Hi = "Hi!"
"""

[<Literal>]
let ExFsharpEnum = """module TopLevel
type EnumTest =
    | None = 0
    | First = 1
"""

let makeIdent name = Ident(name, range.Zero)

let ExFsharpAst = 
    let memberFlags : MemberFlags = {
        IsInstance = true
        IsDispatchSlot = false
        IsOverrideOrExplicitImpl = false
        IsFinal = false
        MemberKind = MemberKind.Member
    }
    let binding : SynBinding = 
        Binding(
            (* SynAccess option *) 
            None,
            (* SynBindingKind *) 
            SynBindingKind.NormalBinding,
            (* IsInline *) 
            false,
            (* IsMutable *) 
            false,
            (* SynAttributes *) 
            SynAttributes.Empty,
            (* PreXmlDoc *) 
            PreXmlDoc.Empty,
            (* SynValData *) 
            SynValData(Some memberFlags, SynValInfo([], SynArgInfo(SynAttributes.Empty, false, None)), None),
            (* SynPat *) 
            SynPat.LongIdent(LongIdentWithDots([makeIdent "x"; makeIdent "Hi"], [range.Zero]), None, None, SynConstructorArgs.Pats[], None, range.Zero),
            (* SynBindingReturnInfo option *) 
            None,
            (* SynExpr *) 
            SynExpr.Const(SynConst.String("Hi!", range.Zero), range.Zero), 
            (* range *) 
            range.Zero, 
            (* SequencePointInfoForBinding *) 
            SequencePointInfoForBinding.NoSequencePointAtInvisibleBinding
    )
    let componentInfo: SynComponentInfo = 
        ComponentInfo(
            (* SynAttributes *) 
            SynAttributes.Empty,
            (* SynTyparDecl list *) 
            List.empty,
            (* SynTypeConstraint list *) 
            List.empty,
            (* LongIdent *) 
            [makeIdent "Foo"],
            (* PreXmlDoc *) 
            PreXmlDoc.Empty, 
            (* PreferPostfix *) 
            false,
            (* SynAccess option *) 
            None,
            (* range *) 
            range.Zero
    )
    let members: SynMemberDefns = [
        SynMemberDefn.ImplicitCtor(None, SynAttributes.Empty, [], None, range.Zero)
        SynMemberDefn.Member(binding, range.Zero)
    ]
    let repr: SynTypeDefnRepr = 
        SynTypeDefnRepr.ObjectModel(
            (* SynTypeDefnKind *) 
            SynTypeDefnKind.TyconUnspecified,
            (* SynMemberSigs *) 
            members,
            (* range *) 
            range.Zero
    )
    let syncType: SynTypeDefn = 
        TypeDefn(
            (* SynComponentInfo *) 
            componentInfo,
            (* SynTypeDefnRepr *) 
            repr,
            (* SynMemberDefns *) 
            List.empty,
            (* range *) 
            range.Zero
    )
    let modules = 
        SynModuleOrNamespace(
            (*longId:LongIdent *) 
            [makeIdent "TopLevel"], //name
            (* isRecursive:bool *) 
            false,
            (* isModule:bool *) 
            true,
            (* decls:SynModuleDecls *)  
            [SynModuleDecl.Types([syncType], range.Zero)],
            (* xmlDoc:PreXmlDoc *) 
            PreXmlDoc.Empty,
            (* attribs:SynAttributes *) 
            SynAttributes.Empty,
            (* accessibility:SynAccess option *) 
            None,
            (* range:range *)
            range.Zero
    )
    let parsedImplFile = 
        ParsedImplFileInput(
            (*fileName*) 
            "tmp.fsx",
            (*isScript*) 
            false,
            (*qualifiedNameOfFile*) 
            QualifiedNameOfFile(makeIdent "TopLevel"),
            (*scopedPragmas: ScopedPragma list*) 
            List.Empty,
            (*hashDirectives : ParsedHashDirective list*) 
            List.Empty, 
            (*modules : SynModuleOrNamespace list*) 
            [modules], 
            ((* isLastCompiland  bool*) true, (* isExe  bool*) true)
    )    
    parsedImplFile |> ParsedInput.ImplFile

let ExFsharpEnumAst = 
    let componentInfo: SynComponentInfo = 
        ComponentInfo(
            (* SynAttributes *) 
            SynAttributes.Empty,
            (* SynTyparDecl list *) 
            List.empty,
            (* SynTypeConstraint list *) 
            List.empty,
            (* LongIdent *) 
            [makeIdent "EnumTest"],
            (* PreXmlDoc *) 
            PreXmlDoc.Empty, 
            (* PreferPostfix *) 
            false,
            (* SynAccess option *) 
            None,
            (* range *) 
            range.Zero
    )
    let enumCases:SynEnumCase list = 
        [
            EnumCase(
                (* SynAttributes *) 
                SynAttributes.Empty,
                (* ident:Ident *) 
                makeIdent "None",
                (* SynConst *) 
                (SynConst.Int32 0),
                (* PreXmlDoc *) 
                PreXmlDoc.Empty,
                (* range:range *) 
                range.Zero
            )
            EnumCase(
                (* SynAttributes *) 
                SynAttributes.Empty,
                (* ident:Ident *) 
                makeIdent "First",
                (* SynConst *) 
                (SynConst.Int32 1),
                (* PreXmlDoc *) 
                PreXmlDoc.Empty,
                (* range:range *) 
                range.Zero
            )
        ]
    let theEnum: SynTypeDefnSimpleRepr = 
        SynTypeDefnSimpleRepr.Enum(
            (* SynEnumCases *)
            enumCases,
            (* range *) 
            range.Zero
    )
    let repr: SynTypeDefnRepr = 
        SynTypeDefnRepr.Simple(theEnum, range.Zero)
    let syncType: SynTypeDefn = 
        TypeDefn(
            (* SynComponentInfo *) 
            componentInfo,
            (* SynTypeDefnRepr *) 
            repr,
            (* SynMemberDefns *) 
            List.empty,
            (* range *) 
            range.Zero
    )
    let modules = 
        SynModuleOrNamespace(
            (*longId:LongIdent *) 
            [makeIdent "TopLevel"], //name
            (* isRecursive:bool *) 
            false,
            (* isModule:bool *) 
            true,
            (* decls:SynModuleDecls *)  
            [SynModuleDecl.Types([syncType], range.Zero)],
            (* xmlDoc:PreXmlDoc *) 
            PreXmlDoc.Empty,
            (* attribs:SynAttributes *) 
            SynAttributes.Empty,
            (* accessibility:SynAccess option *) 
            None,
            (* range:range *)
            range.Zero
    )
    let parsedImplFile = 
        ParsedImplFileInput(
            (*fileName*) 
            "tmp.fsx",
            (*isScript*) 
            false,
            (*qualifiedNameOfFile*) 
            QualifiedNameOfFile(makeIdent "TopLevel"),
            (*scopedPragmas: ScopedPragma list*) 
            List.Empty,
            (*hashDirectives : ParsedHashDirective list*) 
            List.Empty, 
            (*modules : SynModuleOrNamespace list*) 
            [modules], 
            ((* isLastCompiland  bool*) true, (* isExe  bool*) true)
    )    
    parsedImplFile |> ParsedInput.ImplFile

let astTest() =
    let noOriginalSourceCode = "//"
    let config = { FormatConfig.FormatConfig.Default with StrictMode = true }
    // namespace with class, method & return type
    // let myHandCraftedAst = CodeFormatter.FormatAST(ExFsharpAst, noOriginalSourceCode, None, config)
    // enum ast example
    let myHandCraftedAst = CodeFormatter.FormatAST(ExFsharpEnumAst, noOriginalSourceCode, None, config)
    myHandCraftedAst