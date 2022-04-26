﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTest.Core;

[Collection("BlazorTestContext")]
public class BootstrapBlazorTestBase
{
    protected TestContext Context { get; }

    public BootstrapBlazorTestBase()
    {
        Context = BootstrapBlazorTestHost.Instance;
    }
}

[CollectionDefinition("BlazorTestContext")]
public class BootstrapBlazorTestCollection : ICollectionFixture<BootstrapBlazorTestHost>
{

}

public class BootstrapBlazorTestHost : IDisposable
{
    [NotNull]
    internal static TestContext? Instance { get; private set; }

    public BootstrapBlazorTestHost()
    {
        Instance = new TestContext();

        // Mock 脚本
        Instance.JSInterop.Mode = JSRuntimeMode.Loose;

        ConfigureServices(Instance.Services);

        ConfigureConfigration(Instance.Services);

        // 渲染 BootstrapBlazorRoot 组件 激活 ICacheManager 接口
        Instance.Services.GetRequiredService<ICacheManager>();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddBootstrapBlazor();
        services.ConfigureIPLocatorOption(options =>
        {
            options.LocatorFactory = provider => new BaiDuIPLocator();
        });
        services.ConfigureJsonLocalizationOptions(op => op.AdditionalJsonAssemblies = new[] { typeof(Alert).Assembly });
        services.AddSingleton<ILookUpService, FooLookupService>();
    }

    protected virtual void ConfigureConfigration(IServiceCollection services)
    {
        // 增加单元测试 appsettings.json 配置文件
        services.AddConfiguration();
    }

    public void Dispose()
    {
        Instance.Dispose();
        GC.SuppressFinalize(this);
    }

    class FooLookupService : ILookUpService
    {
        public IEnumerable<SelectedItem>? GetItemsByKey(string? key)
        {
            IEnumerable<SelectedItem>? ret = null;

            if (key == "FooLookup")
            {
                ret = new SelectedItem[]
                {
                    new("v1", "LookupService-Test-1"),
                    new("v2", "LookupService-Test-2")
                };
            }
            return ret;
        }
    }
}
