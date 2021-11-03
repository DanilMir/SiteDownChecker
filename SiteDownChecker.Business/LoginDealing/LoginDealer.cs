using System;
using SiteDownChecker.Business.DataBase;
using SiteDownChecker.Business.Models;
using SiteDownChecker.DataAccess;

// ReSharper disable CommentTypo

namespace SiteDownChecker.Business.LoginDealing
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
            var selectResult = DbHelper.SelectWithFilter(typeof(User).ToSqlTableName(),
                new SqlValuePair(nameof(User.Login), user.Login));
            user.Id = selectResult.Count is 0
                ? Guid.NewGuid()
                : (Guid) selectResult[0, nameof(User.Id)];
            return user;
        }

        /// <summary>
        /// важно: работает по айди
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsRegistered(User user) =>
            DbHelper.SelectWithFilter(
                    typeof(User).ToSqlTableName(),
                    new SqlValuePair(nameof(User.Id), user.Id))
                .Count is not 0;

        /// <summary>
        /// важно: работает по айди
        /// выкидывает исключение, если не зарегистрирован
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsPasswordCorrect(User user) =>
            user.Password == (string) DbHelper.SelectWithFilter(
                typeof(User).ToSqlTableName(),
                new SqlValuePair(nameof(User.Id), user.Id))[0, nameof(User.Password)];

        /// <summary>
        /// важно: работает по айди
        /// </summary>
        /// <param name="user"></param>
        public static bool TryRegisterNewUser(User user)
        {
            try
            {
                return DbHelper.SelectWithFilter(
                    typeof(User).ToSqlTableName(), new SqlValuePair(nameof(User.Id), user.Id)).Count is not 0
                    ? throw new Exception($"пользователь с Id ={user.Id} уже зарегистрирован")
                    : Serializer<User>.TrySerialize(user);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
