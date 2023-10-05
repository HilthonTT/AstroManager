namespace AstroManagerApi.Library.DataAccess;
public class DbConnection : IDbConnection
{
    private const string ConnectionId = "MongoDB";
    private readonly IMongoDatabase _db;
    private readonly IConfiguration _config;

    public string DbName { get; private set; }
    public string CredentialCollectionName { get; private set; } = "credentials";
    public string CredentialTemplateCollectionName { get; private set; } = "credential-templates";
    public string UserCollectionName { get; private set; } = "users";
    public string MasterPasswordCollectionName { get; private set; } = "master-passwords";
    public string RecoveryKeyCollectionName { get; private set; } = "recovery-keys";
    public string TypeCollectionName { get; private set; } = "types";

    public MongoClient Client { get; private set; }
    public IMongoCollection<CredentialModel> CredentialCollection { get; private set; }
    public IMongoCollection<CredentialTemplateModel> CredentialTemplateCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<MasterPasswordModel> MasterPasswordCollection { get; private set; }
    public IMongoCollection<RecoveryKeyModel> RecoveryKeyCollection { get; private set; }
    public IMongoCollection<TypeModel> TypeCollection { get; private set; }

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new(_config.GetConnectionString(ConnectionId));
        DbName = _config["MongoDB:DatabaseName"];
        _db = Client.GetDatabase(DbName);

        CredentialCollection = _db.GetCollection<CredentialModel>(CredentialCollectionName);
        CredentialTemplateCollection = _db.GetCollection<CredentialTemplateModel>(CredentialTemplateCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
        MasterPasswordCollection = _db.GetCollection<MasterPasswordModel>(MasterPasswordCollectionName);
        RecoveryKeyCollection = _db.GetCollection<RecoveryKeyModel>(RecoveryKeyCollectionName);
        TypeCollection = _db.GetCollection<TypeModel>(TypeCollectionName);
    }
}
