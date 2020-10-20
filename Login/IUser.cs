﻿using System.Collections.Generic;
 using DNP1_A1.Login;

 namespace DNP1_A1.Login
{
    public interface IUser
    {
        User ValidateUser(string username, string password);
        void AddUser(string username, string password, string role);
        List<User> getUsers();
        void RemoveUser(User toRemove);
    }
}