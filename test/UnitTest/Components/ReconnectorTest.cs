// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components.Web;

namespace UnitTest.Components;

public class ReconnectorTest : BootstrapBlazorTestBase
{
    [Fact]
    public void ReconnectorOutlet_Ok()
    {
        var cut = Context.RenderComponent<ReconnectorOutlet>();
        cut.Contains("components-reconnect-modal");
    }

    [Fact]
    public void ReconnectorContent_Ok()
    {
        var cut = Context.RenderComponent<ReconnectorContent>();
        cut.Contains("components-reconnect-modal");
    }
}
