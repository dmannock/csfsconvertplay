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

[<AutoOpen>]
module AstBuilders =
    let makeIdent name = Ident(name, range.Zero)

    let createdParsedFile modules =
        let parsedImplFile = 
            ParsedImplFileInput(
                (*fileName*) 
                "tmp.fsx",
                (*isScript*) 
                false,
                (*qualifiedNameOfFile*) 
                QualifiedNameOfFile(makeIdent "tmpfile"),
                (*scopedPragmas: ScopedPragma list*) 
                List.Empty,
                (*hashDirectives : ParsedHashDirective list*) 
                List.Empty, 
                (*modules : SynModuleOrNamespace list*) 
                modules, 
                ((* isLastCompiland  bool*) true, (* isExe  bool*) true)
        )    
        parsedImplFile |> ParsedInput.ImplFile

    let createModule name types =
        SynModuleOrNamespace(
            (*longId:LongIdent *) 
            [makeIdent name],
            (* isRecursive:bool *) 
            false,
            (* isModule:bool *) 
            true,
            (* decls:SynModuleDecls *)  
            types,
            (* xmlDoc:PreXmlDoc *) 
            PreXmlDoc.Empty,
            (* attribs:SynAttributes *) 
            SynAttributes.Empty,
            (* accessibility:SynAccess option *) 
            None,
            (* range:range *)
            range.Zero
        )

    let createComponentInfo name =
        ComponentInfo(
            (* SynAttributes *) 
            SynAttributes.Empty,
            (* SynTyparDecl list *) 
            List.empty,
            (* SynTypeConstraint list *) 
            List.empty,
            (* LongIdent *) 
            [makeIdent name],
            (* PreXmlDoc *) 
            PreXmlDoc.Empty, 
            (* PreferPostfix *) 
            false,
            (* SynAccess option *) 
            None,
            (* range *) 
            range.Zero
        )

    let createTypeDef componentInfo repr = 
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

    let createEnumCase name value = 
        EnumCase(
            (* SynAttributes *) 
            SynAttributes.Empty,
            (* ident:Ident *) 
            makeIdent name,
            (* SynConst *) 
            (SynConst.Int32 value),
            (* PreXmlDoc *) 
            PreXmlDoc.Empty,
            (* range:range *) 
            range.Zero
    )

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
    let componentInfo: SynComponentInfo = createComponentInfo "Foo"
    let synType: SynTypeDefn = createTypeDef componentInfo repr
    let types = [SynModuleDecl.Types([synType], range.Zero)]
    let modules = [createModule "TopLevel" types]  
    createdParsedFile modules  

let ExFsharpEnumAst = 
    let enumCases:SynEnumCase list = 
        [
            createEnumCase "None" 0
            createEnumCase "First" 1
        ]
    let theEnum: SynTypeDefnSimpleRepr = 
        SynTypeDefnSimpleRepr.Enum(
            (* SynEnumCases *)
            enumCases,
            (* range *) 
            range.Zero
    )
    let repr: SynTypeDefnRepr = SynTypeDefnRepr.Simple(theEnum, range.Zero)
    let componentInfo = createComponentInfo "EnumTest"
    let synType: SynTypeDefn = createTypeDef componentInfo repr
    let types = [SynModuleDecl.Types([synType], range.Zero)]
    let modules = [createModule "TopLevel" types]  
    createdParsedFile modules

let astTest ast =
    let noOriginalSourceCode = "//"
    let config = { FormatConfig.FormatConfig.Default with StrictMode = true }
    let fsharpCode = CodeFormatter.FormatAST(ast, noOriginalSourceCode, None, config)
    fsharpCode