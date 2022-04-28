// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace UnitTest.Components;

public class DragDropTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Drag_Ok()
    {
        var cut = Context.RenderComponent<Dropzone<string>>(pb =>
        {
            pb.Add(a => a.Items, new List<string> { "1", "2" });
            pb.Add(a => a.MaxItems, 3);
            pb.Add(a => a.ChildContent, v => builder => builder.AddContent(0, v));
        });
        cut.Contains("bb-dd-dropzone");
    }

    [Fact]
    public void ItemWrapperClass_Ok()
    {
        var cut = Context.RenderComponent<Dropzone<string>>(pb =>
        {
            pb.Add(a => a.Items, new List<string> { "1", "2" });
            pb.Add(a => a.ItemWrapperClass, v => "test-ItemWrapperClass");
            pb.Add(a => a.ChildContent, v => builder => builder.AddContent(0, v));
        });
        cut.Contains("test-ItemWrapperClass");
    }
    }
}
