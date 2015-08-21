﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Collections;
using Lib.Core;
using System.IO;
using System.Reflection;

namespace Lib.Web.Utility
{
    /// <summary>
    /// WebControl开发帮助类
    /// </summary>
    /// <remarks>
    /// 包含一些在WebControl开发中所需的静态函数
    /// </remarks>
    public static class WebControlUtility
    {
        private static V GetViewStateValueInternal<V>(StateBag viewState, string key, V nullValue, bool setNullValue, bool isTrackingViewState)
        {
            if (viewState[key] == null)
            {
                if (setNullValue)
                {
                    if (isTrackingViewState)
                    {
                        IStateManager sm = nullValue as IStateManager;
                        if (sm != null)
                        {
                            sm.TrackViewState();
                        }
                    }
                    viewState[key] = nullValue;
                }

                return nullValue;
            }
            return (V)viewState[key];
        }

        /// <summary>
        /// 获取ViewSate中某一项的值
        /// </summary>
        /// <typeparam name="V">返回值得类型</typeparam>
        /// <param name="viewState">ViewSate</param>
        /// <param name="key">某一项的Key</param>
        /// <param name="nullValue">如果此项的值为空，则返回此默认值</param>
        /// <returns>返回值</returns>
        /// <remarks>用于控件开发中，获取属性值</remarks>
        public static V GetViewStateValue<V>(StateBag viewState, string key, V nullValue)
        {
            return GetViewStateValueInternal<V>(viewState, key, nullValue, false, false);
        }

        /// <summary>
        ///	设置ViewSate中某一项的值
        /// </summary>
        /// <typeparam name="V">设置值得类型</typeparam>
        /// <param name="viewState">ViewSate</param>
        /// <param name="key">某一项的Key</param>
        /// <param name="value">设置的值</param>
        /// <remarks>用于控件开发中，设置属性值</remarks>
        public static void SetViewStateValue<V>(StateBag viewState, string key, V value)
        {
            viewState[key] = value;
            IStateManager sm = value as IStateManager;
            if (sm != null)
                sm.TrackViewState();
        }

        /// <summary>
        /// 将ViewState中所有IStateManager类型项，设置TrackViewState
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <remarks>
        /// 在控件的TrackViewState中调用		
        /// </remarks>
        internal static void TrackViewState(StateBag viewState)
        {
            foreach (string key in viewState.Keys)
            {
                IStateManager o = viewState[key] as IStateManager;
                if (o != null)
                {
                    o.TrackViewState();
                }
            }
        }

        /// <summary>
        /// 在LoadViewState之前缓存ViewState中所有IStateManager类型项
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <returns>缓存结果</returns>		
        /// <remarks>
        /// 在LoadViewState之前调用
        /// </remarks>
        /// <example>
        /// <![CDATA[
        ///protected override void LoadViewState(object savedState)
        ///{
        ///    StateBag backState = WebControlUtility.PreLoadViewState(ViewState);
        ///    base.LoadViewState(savedState);
        ///    WebControlUtility.AfterLoadViewState(ViewState, backState);
        ///}
        /// ]]>
        /// </example>
        internal static StateBag PreLoadViewState(StateBag viewState)
        {
            StateBag backState = new StateBag();
            foreach (string key in viewState.Keys)
            {
                IStateManager o = viewState[key] as IStateManager;
                if (o != null)
                {
                    backState[key] = o;
                }
            }
            return backState;
        }

        /// <summary>
        /// 在LoadViewState之后从缓存中恢复将ViewState中ViewSateItemInternal类型项恢复成IStateManager类型项
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <param name="backState">PreLoadViewState产生缓存的项</param>
        /// <remarks>在LoadViewState之后调用</remarks>
        /// <example>
        /// <![CDATA[
        ///protected override void LoadViewState(object savedState)
        ///{
        ///    StateBag backState = WebControlUtility.PreLoadViewState(ViewState);
        ///    base.LoadViewState(savedState);
        ///    WebControlUtility.AfterLoadViewState(ViewState, backState);
        ///}
        /// ]]>
        /// </example>
        //internal static void AfterLoadViewState(StateBag viewState, StateBag backState)
        //{
        //    foreach (string key in viewState.Keys)
        //    {
        //        ViewSateItemInternal vTemp = viewState[key] as ViewSateItemInternal;
        //        if (vTemp != null)
        //        {
        //            if (backState[key] != null)
        //            {
        //                viewState[key] = backState[key];
        //                ((IStateManager)viewState[key]).LoadViewState(vTemp.State);
        //            }
        //            else
        //                viewState[key] = ((ViewSateItemInternal)viewState[key]).GetObject();
        //        }
        //    }
        //}

        /// <summary>
        /// 在SaveViewState之前，将ViewState中所有IStateManager类型项转换为可序列化的ViewSateItemInternal类型项
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <remarks>在SaveViewState之前调用</remarks>		
        /// <example>
        /// <![CDATA[
        ///protected override object SaveViewState()
        ///{
        ///    WebControlUtility.PreSaveViewState(ViewState);
        ///    object o = base.SaveViewState();
        ///    WebControlUtility.AfterSavedViewState(ViewState);
        ///    return o;
        ///}
        /// ]]>
        /// </example>
        //internal static void PreSaveViewState(StateBag viewState)
        //{
        //    foreach (string key in viewState.Keys)
        //    {
        //        IStateManager o = viewState[key] as IStateManager;
        //        if (o != null)
        //        {
        //            viewState[key] = new ViewSateItemInternal(o);
        //        }
        //    }
        //}

        /// <summary>
        /// 在SaveViewState之后，将ViewState中所有ViewSateItemInternal类型项恢复成IStateManager类型项
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <remarks>在SaveViewState之后
        /// 
        /// 调用</remarks>		
        /// <example>
        /// <![CDATA[
        ///protected override object SaveViewState()
        ///{
        ///    WebControlUtility.PreSaveViewState(ViewState);
        ///    object o = base.SaveViewState();
        ///    WebControlUtility.AfterSavedViewState(ViewState);
        ///    return o;
        ///}
        /// ]]>
        /// </example>
        //internal static void AfterSavedViewState(StateBag viewState)
        //{
        //    foreach (string key in viewState.Keys)
        //    {
        //        object o = viewState[key];
        //        if (o is ViewSateItemInternal)
        //        {
        //            viewState[key] = ((ViewSateItemInternal)o).GetObject();
        //        }
        //    }
        //}

        /// <summary>
        /// 在containerControl中查找类型为controlType的控件
        /// </summary>
        /// <param name="containerControl">父控件</param>
        /// <param name="controlType">查找的控件类型</param>
        /// <param name="deepFind">是否进行深度查找</param>
        /// <returns>找到的控件</returns>
        /// <remarks>在containerControl中查找类型为controlType的控件</remarks>
        public static Control FindControl(Control containerControl, Type controlType, bool deepFind)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(containerControl != null, "containerControl");
            ExceptionHelper.FalseThrow<ArgumentNullException>(controlType != null, "controlType");

            Control result = null;
            foreach (Control ctr in containerControl.Controls)
            {
                if (ctr.GetType() == controlType)
                {
                    result = ctr;
                }
                else
                {
                    if (deepFind)
                        result = FindControl(ctr, controlType, deepFind);
                }

                if (result != null)
                    break;
            }

            return result;
        }

        /// <summary>
        /// 在containerControl中查找类型为T的控件
        /// </summary>
        /// <typeparam name="T">查找的控件类型</typeparam>
        /// <param name="containerControl">父控件</param>
        /// <param name="deepFind">是否进行深度查找</param>
        /// <returns>找到的控件</returns>
        /// <remarks>在containerControl中查找类型为controlType的控件</remarks>
        public static T FindControl<T>(Control containerControl, bool deepFind)
            where T : Control
        {
            return (T)FindControl(containerControl, typeof(T), deepFind);
        }

        /// <summary>
        /// 在containerControl中查找ID为controlID的控件
        /// </summary>
        /// <param name="containerControl">父控件</param>
        /// <param name="controlID">子控件的ID</param>
        /// <param name="deepFind">是否进行深度查找</param>
        /// <returns>找到的控件</returns>
        public static Control FindControlByID(Control containerControl, string controlID, bool deepFind)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(containerControl != null, "containerControl");
            ExceptionHelper.CheckStringIsNullOrEmpty(controlID, "controlID");

            Control result = containerControl.FindControl(controlID);

            if (result == null && deepFind)
            {
                foreach (Control innerCtrl in containerControl.Controls)
                {
                    result = FindControlByID(innerCtrl, controlID, deepFind);

                    if (result != null)
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 在containerControl中查找html的id为controlID的控件
        /// </summary>
        /// <param name="containerControl">父控件</param>
        /// <param name="controlID">子控件的html id</param>
        /// <param name="deepFind">是否进行深度查找</param>
        /// <returns>找到的控件</returns>
        public static Control FindControlByHtmlIDProperty(Control containerControl, string controlID, bool deepFind)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(containerControl != null, "containerControl");
            ExceptionHelper.CheckStringIsNullOrEmpty(controlID, "controlID");

            Control result = null;

            if (containerControl is IAttributeAccessor)
            {
                if (string.Compare(containerControl.ID, controlID, true) == 0)
                    result = containerControl;
            }

            if (deepFind)
            {
                if (result == null)
                {
                    foreach (Control innerCtrl in containerControl.Controls)
                    {
                        result = FindControlByHtmlIDProperty(innerCtrl, controlID, true);

                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 查找类型为controlType的父亲及祖先控件
        /// </summary>
        /// <param name="currentControl">当前的控件</param>
        /// <param name="controlType">控件类型</param>
        /// <param name="recursively">是否多级查找</param>
        /// <returns>符合类型的控件</returns>
        public static Control FindParentControl(Control currentControl, Type controlType, bool recursively)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(currentControl != null, "currentControl");

            Control result = currentControl;

            while (result != null && result.GetType() != controlType)
            {
                if (recursively)
                    result = result.Parent;
                else
                    result = null;
            }

            return result;
        }

        /// <summary>
        /// 查找类型为T的父亲及祖先控件
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="currentControl">当前的控件</param>
        /// <param name="recursively">是否多级查找</param>
        /// <returns>符合类型的控件</returns>
        public static T FindParentControl<T>(Control currentControl, bool recursively)
            where T : Control
        {
            return (T)FindParentControl(currentControl, typeof(T), recursively);
        }

        /// <summary>
        /// 得到控件的Html描述
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetControlHtml(Control ctrl)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(ctrl != null, "ctrl");

            StringBuilder strB = new StringBuilder(1024);

            using (StringWriter sw = new StringWriter(strB))
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(sw))
                {
                    ctrl.RenderControl(writer);
                }
            }
            return strB.ToString();
        }
    }
}