﻿namespace AuthService.Utils.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);


    public bool VerifyPassword(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
}
