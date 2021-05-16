using System;

namespace AutoInjection
{
    /// <summary>
    /// 服务注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceInjectionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type InterfaceType { get; set; }

        /// <summary>
        /// 注入类型
        /// </summary>
        public InjectionType InjectionType { get; }

        /// <summary>
        /// 服务注入
        /// </summary>
        public ServiceInjectionAttribute()
        {
            this.InjectionType = InjectionType.Scoped;
        }

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="injectionType">注入类型</param>
        public ServiceInjectionAttribute(InjectionType injectionType)
        {
            this.InjectionType = injectionType;
        }

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="interfaceType">服务的接口类型</param>
        /// <param name="injectionType">注入的类型</param>
        public ServiceInjectionAttribute(Type interfaceType, InjectionType injectionType)
        {
            this.InterfaceType = interfaceType;
            this.InjectionType = injectionType;
        }
    }
}