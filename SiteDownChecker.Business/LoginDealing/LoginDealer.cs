using System;
using MedicalVideo.Business.DataBase;
using MedicalVideo.Business.Models;
using MedicalVideo.DataAccess;

// ReSharper disable CommentTypo

namespace MedicalVideo.Business.LoginDealing
{
    public static class LoginDealer
    {
        /// <summary>
        /// меняет айди на равный найденному, если найдет, иначе на новый айди
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static User SetId(User user)
        {
            var selectResult = DbHelper.SelectRequest($"select * from Users where Login = {user.Login.ToSQLString()}");
            user.Id = selectResult.RowsCount is 0
                ? Guid.NewGuid()
                : new SelectResultAdapter(selectResult).Deserialize<User>(0).Id;
            return user;
        }

        /// <summary>
        /// важно: работает по айди
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsRegistered(User user) =>
            DbHelper.SelectRequest($"select * from Users where Id = {user.Id.ToSQLString()}").RowsCount is not 0;

        /// <summary>
        /// важно: работает по айди
        /// выкидывает исключение, если не зарегистрирован
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsPasswordCorrect(User user) =>
            user.Password ==
            new SelectResultAdapter(DbHelper.SelectRequest($"select * from Users where Id = {user.Id.ToSQLString()}"))
                .Deserialize<User>(0).Password;

        /// <summary>
        /// важно: работает по айди
        /// выкидывет исключение, если уже зарегистрирован
        /// </summary>
        /// <param name="user"></param>
        public static bool TryRegisterNewUser(User user)
        {
            if (DbHelper.SelectRequest($"select * from Users where Id = {user.Id.ToSQLString()}").RowsCount is not 0)
                throw new Exception($"пользователь с Id ={user.Id} уже зарегистрирован");
            Serializer<User>.Serialize(user);
            return true;
        }
    }
}
