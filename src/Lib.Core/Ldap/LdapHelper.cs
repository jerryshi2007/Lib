using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace Lib.Ldap
{
    public class LdapHelper
    {
        private static LdapHelper ldapHelper = null;
        public static LdapHelper GetInstance()
        {
            if (ldapHelper == null)
            {
                ldapHelper = new LdapHelper();
            }
            return ldapHelper;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rootDSE">ldap的地址，例如"LDAP://***.***.48.110:389/dc=***,dc=com"</param>
        /// <param name="authUserName">连接用户名，例如"cn=root,dc=***,dc=com"</param>
        /// <param name="authPwd">连接密码</param>
        public DirectoryEntry OpenConnection(string rootDSE, string userName, string password)
        {
            return new DirectoryEntry(rootDSE, userName, password, AuthenticationTypes.None);
        }


        /// <summary>
        /// 检测一个用户和密码是否正确
        /// </summary>
        /// <param name="ldapFilter">(|(uid= {0})(cn={0}))</param>
        /// <param name="userID">testuserid</param>
        /// <param name="userPwd">testuserpassword</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Authentication(string ldapPath, string ldapFilter, string userID, string userPwd)
        {
            if (string.IsNullOrEmpty(ldapFilter))
                ldapFilter = @"(&(objectclass=person)(|(uid={0})(cn={1})))";
            ldapFilter = string.Format(ldapFilter, userID, userID);

            using (DirectoryEntry rootEntry = OpenConnection(ldapPath, string.Empty, string.Empty))
            {
                bool result = false;
                try
                {
                    DirectorySearcher deSearch = new DirectorySearcher(rootEntry);

                    deSearch.Filter = ldapFilter;
                    deSearch.SearchScope = SearchScope.Subtree;
                    SearchResult entryResult = deSearch.FindOne();

                    //如果用户密码为空
                    if (string.IsNullOrEmpty(userPwd))
                    {
                        result = false;
                    }
                    else
                    {
                        if (entryResult != null && !string.IsNullOrEmpty(entryResult.Path))
                        {
                            //获取用户名路径对应的用户uid
                            int pos = entryResult.Path.LastIndexOf('/');
                            string uid = entryResult.Path.Remove(0, pos + 1);
                            DirectoryEntry userEntry = new DirectoryEntry(entryResult.Path, uid, userPwd, AuthenticationTypes.None);
                            if (userEntry != null && userEntry.Properties.Count > 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                    throw ex;
                }
                return result;
            }
        }
    }
}
