using System;

namespace Pipeline.Testing.Decorators
{
    public class RequiresPermissionAttribute: Attribute {
        public string Permission {get;set;}
    }
}
