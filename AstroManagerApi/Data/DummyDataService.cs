using AstroManagerApi.Data.Interfaces;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Data;

public class DummyDataService : IDummyDataService
{
    private readonly ICredentialTemplateData _templateData;
    private readonly ITypeData _typeData;
    private readonly IUserData _userData;
    private readonly ICredentialData _credentialData;

    public DummyDataService(
        ICredentialTemplateData templateData,
        ITypeData typeData,
        IUserData userData,
        ICredentialData credentialData)
    {
        _templateData = templateData;
        _typeData = typeData;
        _userData = userData;
        _credentialData = credentialData;
    }

    public async Task CreateTypesAsync()
    {
        var types = new List<TypeModel>
        {
            new TypeModel()
            {
                Name = "Logins",
                Description = @"This type is used to store information related to website or application logins. 
                               It typically includes fields for usernames, passwords, and perhaps URLs or other relevant details."
            },
            new TypeModel()
            {
                Name = "Passwords",
                Description = @"This type is meant to securely store passwords. 
                               It could include fields for the password itself along with a description or a label to identify what the password is for."
            },
            new TypeModel()
            {
                Name = "Secure Note",
                Description = @"This type is used for storing sensitive or confidential notes securely. 
                               You can use it to store information like private keys, PINs, or any other sensitive text-based data."
            },
            new TypeModel()
            {
                Name = "Credit Card",
                Description = @"This type is designed to store credit card information. 
                               It would include fields for the credit card number, expiration date, CVV code, and maybe even the cardholder's name."
            },
            new TypeModel()
            {
                Name = "Identity",
                Description = @"This type can be used for storing personal identity information. 
                               It might include fields for your name, date of birth, address, social security number, or any other personal identification data you want to keep secure."
            }
        };

        foreach (var type in types)
        {
            await _typeData.CreateTypeAsync(type);
        }
    }

    public async Task CreateTemplatesAsync()
    {
        var types = await _typeData.GetAllTypesAsync();

        var loginType = types.FirstOrDefault(x => x.Name == "Logins");
        var passwordType = types.FirstOrDefault(x => x.Name == "Passwords");
        var secureNoteType = types.FirstOrDefault(x => x.Name == "Secure Note");
        var creditCardType = types.FirstOrDefault(x => x.Name == "Credit Card");
        var identityType = types.FirstOrDefault(x => x.Name == "Identity");

        var templates = new List<CredentialTemplateModel>()
        {
            new()
            {
                Type = loginType,
                Fields = new List<FieldModel>
                {
                    new FieldModel()
                    {
                        Name = "Username",
                        Value = "",
                    },
                    new FieldModel()
                    {
                        Name = "Password",
                        Value = "",
                    },
                    new FieldModel()
                    {
                        Name = "Website",
                        Value = "",
                    },
                    new FieldModel()
                    {
                        Name = "Notes",
                        Value = "",
                    }
                }
            },
            new()
            {
                Type = passwordType,
                Fields = new List<FieldModel>
                {
                    new()
                    {
                        Name = "Password",
                        Value = "",
                    },
                    new FieldModel()
                    {
                        Name = "Website",
                        Value = "",
                    },
                    new FieldModel()
                    {
                        Name = "Notes",
                        Value = "",
                    }
                },
            },
            new()
            {
                Type = secureNoteType,
                Fields = new()
                {
                    new()
                    {
                        Name = "Notes",
                        Value = "",
                    }
                }
            },

            new()
            {
                Type = creditCardType,
                Fields = new()
                {
                    new()
                    {
                        Name = "Cardholder name",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Type",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Verification Number",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Expiry Date",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Valid From",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Notes",
                        Value = "",
                    }
                }
            },
            new()
            {
                Type = identityType,
                Fields = new()
                {
                    new()
                    {
                        Name = "First Name",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Initial",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Last Name",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Gender",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Birth Date",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Occupation",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Company",
                        Value = "",
                    },
                    new()
                    {
                        Name = "Department",
                        Value = "",
                    },
                }
            }
        };


        foreach (var t in templates)
        {
            await _templateData.CreateTemplateAsync(t);
        }
    }

    public async Task CreateDummyCredentialsAsync()
    {
        string id = "650343d1ba55c4749132df10";
        var user = await _userData.GetUserAsync(id);
        var types = await _typeData.GetAllTypesAsync();
        var templates = await _templateData.GetAllTemplatesAsync();

        var loginType = types.FirstOrDefault(x => x.Name == "Logins");
        var loginTemplate = templates.FirstOrDefault(x => x.Type.Name == loginType?.Name);

        var credential = new CredentialModel()
        {
            Title = "Game Login",
            Type = loginType,
            User = new BasicUserModel(user),
            Fields = loginTemplate?.Fields,
        };

        await _credentialData.CreateCredentialAsync(credential);
    }
}
