# AutoInjection使用说明

1. 服务配置

   在```Startup```中的```ConfigureServices```函数中配置服务，如下所示：

   ```c#
   public void ConfigureServices(IServiceCollection services)
   {
       // 声明为WebAPI，也可为AddMvc()或者其他
       services.AddControllers();
       // 配置其他服务
       // 配置自动注入服务
       services.ServicesAutoInjection();
       // 配置其他服务
   }
   ```

2. 服务注入

   配置服务的注入，在需要注入的业务类中，使用```ServiceInjectionAttribute```标记，从而实现注入：

   ```c#
   [ServiceInjection(typeof(IService),InjectionType.Scoped)]
   public class ServiceImpl : IService
   {
   	// 业务代码
   }
   ```
3. 接口说明
   ```ServiceInjectionAttribute```有三种实现方法：
   |                         实现方法                          |                           使用说明                           |
   | :-------------------------------------------------------: | :----------------------------------------------------------: |
   |                    [ServiceInjection]                     | 默认注入，注入的生命周期为Scoped，服务的接口该类继承的第一个接口 |
   |         [ServiceInjection(InjectionType.Scoped)]          |       指定生命周期注入，服务的接口该类继承的第一个接口       |
   | [ServiceInjection(typeof(IService),InjectionType.Scoped)] |                 指定生命周期和服务接口的注入                 |
