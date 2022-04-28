﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// Responsives 示例类
/// </summary>
public partial class Responsives
{
    private BreakPoint Size { get; set; }

    private Task OnChanged(BreakPoint size)
    {
        Size = size;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        new AttributeItem()
        {
            Name = nameof(Responsive.OnBreakPointChanged),
            Description = "断点阀值改变时回调方法",
            Type = "Func<BreakPoint, Task<bool>>",
            ValueList = " — ",
            DefaultValue = " — "
        }
    };
}
