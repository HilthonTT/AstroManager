using AstroManagerApi.Library.Enums;

namespace AstroManagerApi.Library.Helpers;
public static class SqlHelper
{
    public static string GetStoredProcedure(DataType datatype, Operation operation)
    {
        string sp = datatype switch
        {
            DataType.Attribute => "spAttribute",
            DataType.Entity => "spEntity",
            DataType.EntityAttribute => "spEntityAttribute",
            DataType.MasterPassword => "spMasterPassword",
            DataType.RecoveryKey => "spRecoveryKey",
            DataType.User => "spUser",
            _ => "",
        };

        string op = operation switch
        {
            Operation.Create => "Insert",
            Operation.Update => "Update",
            Operation.GetById => "GetById",
            Operation.GetByOid => "GetByOid",
            Operation.Delete => "Delete",
            _ => "",
        };

        return $"dbo.{sp}_{op}";
    }
}
