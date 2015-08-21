using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lib.Web.Utility;

namespace Lib.Web.Controls
{
    public abstract class LibWebControl : WebControl, INamingContainer, IPostBackDataHandler, IPostBackEventHandler, ICallbackEventHandler
    {
        #region Private

        private bool _enableClientState;
        private string _tagName;
        private HtmlTextWriterTag _tagKey;

        #endregion

        #region Field

        /// <summary>
        /// 是否只读
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("是否只读")]
        public bool ReadOnly
        {
            get { return GetPropertyValue<bool>("ReadOnly", false); }
            set { SetPropertyValue<bool>("ReadOnly", value); }
        }

        #endregion

        #region [ Constructor ]

        /// <summary>
		/// 构造函数
        /// Initializes a new ScriptControl
        /// </summary>
        /// <param name="tag"></param>
        public LibWebControl(HtmlTextWriterTag tag)
            : this(false, tag)
        {
			
        }

        /// <summary>
		/// 构造函数
        /// Initializes a new ScriptControl
        /// </summary>
        protected LibWebControl()
            : this(true)
        {
        }

        /// <summary>
		/// 构造函数
        /// Initializes a new ScriptControl
        /// </summary>
        /// <param name="tag">控件的tagName</param>
        protected LibWebControl(string tag)
            : this(false, tag)
        {
        }

        /// <summary>
		/// 构造函数
        /// Initializes a new ScriptControl
        /// </summary>
        /// <param name="enableClientState">是否使用ClientState</param>
        protected LibWebControl(bool enableClientState)
        {
            _enableClientState = enableClientState;
        }
		 
        /// <summary>
		/// 构造函数
        /// Initializes a new ScriptControl
        /// </summary>
		/// <param name="enableClientState">是否使用ClientState</param>
		/// <param name="tag">控件的HtmlTextWriterTag</param>
        protected LibWebControl(bool enableClientState, HtmlTextWriterTag tag)
        {
            _tagKey = tag;
            _enableClientState = enableClientState;		
        }

        /// <summary>
		/// 构造函数
        /// Initializes a new ScriptControl
        /// </summary>
		/// <param name="enableClientState">是否使用ClientState</param>
		/// <param name="tag">控件的tagName</param>
        protected LibWebControl(bool enableClientState, string tag)
        {
            _tagKey = HtmlTextWriterTag.Unknown;
            _tagName = tag;
            _enableClientState = enableClientState;         
        }
        #endregion

        #region Protect

        /// <summary>
        /// 从ViewState中获取某属性值，如果为空则返回默认值nullValue
        /// </summary>
        /// <typeparam name="V">属性类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="nullValue">默认值</param>
        /// <returns>属性值</returns>
        /// <remarks>从ViewState中获取某属性值，如果为空则返回默认值nullValue</remarks>
        protected V GetPropertyValue<V>(string propertyName, V nullValue)
        {
            return WebControlUtility.GetViewStateValue<V>(ViewState, propertyName, nullValue);
        }

        /// <summary>
        /// 设置某属性的值
        /// </summary>
        /// <typeparam name="V">属性类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        /// <remarks>设置某属性的值</remarks>
        protected void SetPropertyValue<V>(string propertyName, V value)
        {
            WebControlUtility.SetViewStateValue<V>(ViewState, propertyName, value);
        }

        #endregion

        #region IPostBackDataHandler

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            throw new NotImplementedException();
        }

        public void RaisePostDataChangedEvent()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPostBackEventHandler

        public void RaisePostBackEvent(string eventArgument)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICallbackEventHandler

        public string GetCallbackResult()
        {
            throw new NotImplementedException();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            throw new NotImplementedException();
        }

        #endregion 
    }
}
