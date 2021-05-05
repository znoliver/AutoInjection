using System;

namespace AutoInjection
{
    /// <summary>
    /// 未接口异常
    /// </summary>
    public class NoImplementationInterfaceException : Exception
    {
        /// <summary>
        /// 异常消息
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public NoImplementationInterfaceException(string message)
        {
            this.Message = message;
        }
    }
}