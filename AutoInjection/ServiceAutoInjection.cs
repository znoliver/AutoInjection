using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInjection
{
    /// <summary>
    /// 自动服务注入
    /// </summary>
    public static class AutoInjection
    {
        /// <summary>
        /// 服务自动注入
        /// </summary>
        /// <param name="serviceCollection">需要自动注入服务的服务集合</param>
        /// <exception cref="ArgumentOutOfRangeException">指定的注入类型不在可注入的范围内</exception>
        /// <exception cref="NoImplementationInterfaceException">指定注入的类型未实现任何服务</exception>
        /// <exception cref="ArgumentException">输入的参数错误：1、注入的类型未实现指定的服务。2、指定的服务不是Interface类型</exception>
        /// <returns>自动注入服务后的服务集合</returns>
        public static IServiceCollection ServicesAutoInjection(this IServiceCollection serviceCollection)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var types = Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(Assembly.LoadFrom)
                .SelectMany(a => a.GetTypes());

            Injection(serviceCollection, types);

            return serviceCollection;
        }

        /// <summary>
        /// 服务自动注入
        /// </summary>
        /// <param name="serviceCollection">需要自动注入服务的服务集合</param>
        /// <param name="selector">应用于每个Assembly的筛选函数</param>
        /// <exception cref="ArgumentOutOfRangeException">指定的注入类型不在可注入的范围内</exception>
        /// <exception cref="NoImplementationInterfaceException">指定注入的类型未实现任何服务</exception>
        /// <exception cref="ArgumentException">输入的参数错误：1、注入的类型未实现指定的服务。2、指定的服务不是Interface类型</exception>
        /// <returns>自动注入服务后的服务集合</returns>
        public static IServiceCollection ServicesAutoInjection(this IServiceCollection serviceCollection, Func<Assembly, bool> selector)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var types = Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(Assembly.LoadFrom)
                .Where(selector)
                .SelectMany(a => a.GetTypes());

            Injection(serviceCollection, types);

            return serviceCollection;
        }

        // 注入逻辑
        private static void Injection(IServiceCollection serviceCollection, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<ServiceInjection>();
                if (attribute == null) continue;
                if (attribute.InterfaceType == null)
                {
                    var interfaceList = type.GetInterfaces();
                    if (interfaceList.Length == 0) throw new NoImplementationInterfaceException($"该类型未实现任何服务(Type:{type.Name})。");
                    attribute.InterfaceType = interfaceList.First();
                }
                else
                {
                    var isInterface = attribute.InterfaceType.IsInterface;
                    if (!isInterface) throw new ArgumentException("指定的服务不是Interface类型。");
                    var isRealization = type.IsAssignableFrom(attribute.InterfaceType);
                    if (!isRealization) throw new ArgumentException("注入的类型未实现指定的服务。");
                }

                switch (attribute.InjectionType)
                {
                    case InjectionType.Transient:
                        serviceCollection.AddTransient(attribute.InterfaceType, type);
                        break;
                    case InjectionType.Scoped:
                        serviceCollection.AddScoped(attribute.InterfaceType, type);
                        break;
                    case InjectionType.Singleton:
                        serviceCollection.AddSingleton(attribute.InterfaceType, type);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}