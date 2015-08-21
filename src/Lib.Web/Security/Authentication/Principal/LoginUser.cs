using System;
using System.Collections.Generic;
using Lib.Web.Json;
using Lib.Config;
using Lib.Core;

namespace Lib.Web.Security
{
    public abstract class LoginUser : ILoginUser
    {
        #region Construct

        public LoginUser(string loginName)
        {
            this.loginName = loginName;
        }

        public LoginUser(string applicationID, string userID, string loginName, string realName, string displayName, string originalUserID, string domain)
        {
            this.applidationID = applicationID;
            this.userID = userID;
            this.loginName = loginName;
            this.realName = realName;
            this.displayName = displayName;
            this.originalUserID = originalUserID;
            this.domain = domain;
        }

        #endregion

        #region Private Field

        private string applidationID;
        private string userID;
        private string loginName;
        private string realName;
        private string displayName;
        private string originalUserID;
        private string domain;

        private Dictionary<string, object> properities = null;

        #endregion Private Field

        #region Public Field

        public string ApplicationID
        {
            get
            {
                return this.applidationID;
            }
            set
            {
                this.applidationID = value;
            }
        }

        public string UserID
        {
            get
            {
                return this.userID;
            }
            set
            {
                this.userID = value;
            }
        }

        public string LoginName
        {
            get
            {
                return this.loginName;
            }
            set
            {
                this.loginName = value;
            }
        }

        public string RealName
        {
            get
            {
                return this.realName;
            }
            set
            {
                this.realName = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }

        public string OriginalUserID
        {
            get
            {
                return this.originalUserID;
            }
            set
            {
                this.originalUserID = value;
            }
        }

        public string Domain
        {
            get
            {
                return this.domain;
            }
            set
            {
                this.domain = value;
            }
        }



        public Dictionary<string, object> Properties
        {
            get
            {
                if (this.properities == null)
                    this.properities = new Dictionary<string, object>();
                return this.properities;
            }
        }

        #endregion Public Field

        #region Public Method

        public string Serialize()
        {
            return JsonHelper.Serialize(this);
        }

        public ILoginUser Deserialize(string dataString)
        {
            return JsonHelper.Deserialize<ILoginUser>(dataString);
        }

        public abstract bool IsInRole(string role);

        public abstract ILoginUser Check(string password);

                
        #endregion Public Method

        /// <summary>
        /// 创建登录对象，未认证
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static ILoginUser CreateLoginUser(string loginName)
        {
            TypeConfigurationElement typeElem = AuthenticationClientSection.GetConfig().LoginUserType;

            ExceptionHelper.TrueThrow(typeElem == null, "未指定登录信息类型");

            ILoginUser loginUser = typeElem.CreateInstance(loginName) as ILoginUser;

            ExceptionHelper.TrueThrow(loginUser == null, "指定的登录类型无法进行实例化");

            return loginUser;
        }

    
    }
}
