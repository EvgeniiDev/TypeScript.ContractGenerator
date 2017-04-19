using SKBKontur.Catalogue.FlowType.CodeDom;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.Internals
{
    internal class FlowTypeImportDefaultFromPathStatement : FlowTypeImportStatement
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("import {0} from '{1}';", TypeName, context.GetReferenceFromUnitToAnother(CurrentUnit.Path, PathToUnit));
        }

        public string TypeName { get; set; }
        public FlowTypeUnit CurrentUnit { get; set; }
        public string PathToUnit { get; set; }
    }
}