using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Models;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoUserData : IUserData
{
    private readonly IMongoCollection<UserModel> _users;
    public MongoUserData(IDbConnection db)
    {
        _users = db.UserCollection;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var results = await _users.FindAsync(_ => true);
        return await results.ToListAsync();
    }

    public async Task<UserModel> GetUserAsync(string id)
    {
        var results = await _users.FindAsync(u => u.Id == id);
        return await results.FirstOrDefaultAsync();
    }

    public async Task<UserModel> GetUserFromAuthAsync(string objectId)
    {
        var results = await _users.FindAsync(u => u.ObjectIdentifier == objectId);
        return await results.FirstOrDefaultAsync();
    }

    public async Task<UserModel> CreateUserAsync(UserModel user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
        await _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }
}
