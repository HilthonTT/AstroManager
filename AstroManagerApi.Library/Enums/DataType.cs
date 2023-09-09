namespace AstroManagerApi.Library.Enums;
public enum DataType
{
    Attribute,
    Entity,
    EntityAttribute,
    MasterPassword,
    RecoveryKey,
    User,
}

public enum Operation
{
    Create,
    Update,
    GetAll,
    GetByOid,
    GetById,
    GetByUserId,
    Delete,
}
