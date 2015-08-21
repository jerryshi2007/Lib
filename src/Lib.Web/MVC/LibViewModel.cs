using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Lib.Web.Mvc
{
    public class LibViewModel
    {
        public LibViewModel()
        { }

        public LibViewModel(LibViewModelType modelType)
            : this(modelType, LibViewModelCode.Success)
        { }

        public LibViewModel(LibViewModelType modelType, LibViewModelCode resultCode)
            : this(modelType, resultCode, string.Empty)
        { }

        public LibViewModel(LibViewModelType resultMode, LibViewModelCode resultCode, string errorMessage)
        {
            this.ModeType = resultMode;
            this.ResponseCode = resultCode;
            this.ErrorMessage = errorMessage;
        }

        public static LibViewModel CreateSuccessView(LibViewModelType viewType)
        {
            return new LibViewModel(viewType, LibViewModelCode.Success);
        }

        public static LibViewModel CreateFailedView(LibViewModelType viewType , string message)
        {
            return new LibViewModel(viewType, LibViewModelCode.Failed, message);          
        }

        public LibViewModelType ModeType { get; set; }

        [ClientPropertyName("error")]
        public string ErrorMessage { get; set; }

        [ClientPropertyName("html")]
        public string HtmlString { get; set; }

        [ClientPropertyName("code")]
        public LibViewModelCode ResponseCode { get; set; }
    }

    public enum LibViewModelCode
    {
        Success = 1,
        Failed = -1
    }

    public enum LibViewModelType
    {
        /// <summary>
        /// 返回Json字符串
        /// </summary>
        Json = 0,

        /// <summary>
        /// 返回Html字符串
        /// </summary>
        Html = 1,  

        /// <summary>
        /// 返回文件流
        /// </summary>
        File = 2, 

        /// <summary>
        /// 拒绝访问
        /// </summary>
        Detect = 3
    }
}
