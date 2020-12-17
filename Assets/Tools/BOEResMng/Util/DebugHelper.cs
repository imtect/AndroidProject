///<summary>
    ///<para>Copyright (C)京东方科技集团有限公司</para>
    /// <para>文件功能：可以统一打开关闭的输出日志 </para>
    /// <para>开发部门：IOT软件研发中心</para>
    /// <para>创 建 人： </para>
    /// <para>创建日期：</para>
    /// <para>修 改 人：</para>
    /// <para>修改日期：</para>
/// </summary>
using UnityEngine;
namespace BOE.BOEComponent.Util
{
    public static class DebugHelper
    {
        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(object message)
        {
#if Debug
            Debug.LogError(message.ToString());
#endif
        }

        /// <summary>
        /// 输出警告信息
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(object message)
        {
#if Debug
            Debug.LogWarning(message.ToString());
#endif
        }

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="message"></param>
        public static void Log(object message)
        {
#if Debug
            Debug.Log(message.ToString());
#endif
        }
    }
}