module Output
open Microsoft.FSharp.Compiler.Ast
open Microsoft.FSharp.Compiler.Range
open Fantomas

[<Literal>]
let ExFsharp = """module TopLevel
type Foo() = 
    member x.Hi = "Hi!"
"""

let makeIdent name = Ident(name, range.Zero)

let myAst = 
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

let astTest() =
    let noOriginalSourceCode = "//"
    let config = { FormatConfig.FormatConfig.Default with StrictMode = true }
    let myHandCraftedAst = CodeFormatter.FormatAST(myAst, noOriginalSourceCode, None, config)
    myHandCraftedAst