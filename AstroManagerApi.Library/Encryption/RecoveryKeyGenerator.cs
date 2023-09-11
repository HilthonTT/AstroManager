﻿using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Models;
using AstroManagerApi.Library.Models.Request;
using System.Security.Cryptography;

namespace AstroManagerApi.Library.Encryption;
public class RecoveryKeyGenerator : IRecoveryKeyGenerator
{
    private const int NumberOfKeys = 3;
    private readonly ITextHasher _hasher;

    public RecoveryKeyGenerator(ITextHasher hasher)
    {
        _hasher = hasher;
    }

    private static string GenerateRecoveryKey()
    {
        byte[] keyBytes = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        return Convert.ToBase64String(keyBytes);
    }

    public RecoveryRequestModel GenerateRequest()
    {
        var request = new RecoveryRequestModel();
        for (int i = 0; i < NumberOfKeys; i++)
        {
            string plainkey = GenerateRecoveryKey();
            request.PlainRecoveryKeys.Add(plainkey);
        }

        foreach (var key in request.PlainRecoveryKeys)
        {
            string hashedKey = _hasher.HashPlainText(key);
            request.Recovery.RecoveryKeys.Add(hashedKey);
        }

        return request;
    }

    public bool VerifyKey(string key, string hashedKey)
    {
        return _hasher.VerifyPassword(key, hashedKey);
    }
}
