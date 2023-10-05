namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IDbConnection
{
    MongoClient Client { get; }
    IMongoCollection<CredentialModel> CredentialCollection { get; }
    string CredentialCollectionName { get; }
    IMongoCollection<CredentialTemplateModel> CredentialTemplateCollection { get; }
    string CredentialTemplateCollectionName { get; }
    string DbName { get; }
    IMongoCollection<MasterPasswordModel> MasterPasswordCollection { get; }
    string MasterPasswordCollectionName { get; }
    IMongoCollection<RecoveryKeyModel> RecoveryKeyCollection { get; }
    string RecoveryKeyCollectionName { get; }
    IMongoCollection<UserModel> UserCollection { get; }
    string UserCollectionName { get; }
    IMongoCollection<TypeModel> TypeCollection { get; }
    string TypeCollectionName { get; }
}