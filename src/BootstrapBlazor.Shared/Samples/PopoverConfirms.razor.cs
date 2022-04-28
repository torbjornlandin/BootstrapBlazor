﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using BootstrapBlazor.Shared.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// 
/// </summary>
public sealed partial class PopoverConfirms
{
    [NotNull]
    private Foo? Model { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    private BlockLogger? Trace { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    private BlockLogger? Trace1 { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Model = new() { Name = "Name", Education = EnumEducation.Primary, DateTime = DateTime.Now };
    }

    /// <summary>
    /// 
    /// </summary>
    private Task OnClose()
    {
        // 点击确认按钮后此方法被回调，点击取消按钮时此方法不会被调用
        Trace.Log("OnClose Trigger");
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    private Task OnConfirm()
    {
        // 点击确认按钮后此方法被回调，点击取消按钮时此方法不会被调用
        Trace.Log("OnConfirm Trigger");
        return Task.CompletedTask;
    }

    private static Task OnAsyncConfirm() => Task.Delay(3000);

    private async Task OnAsyncSubmit()
    {
        await Task.Delay(3000);
        Trace1.Log("异步提交");
    }

    private Task OnValidSubmit(EditContext context)
    {
        Trace1.Log("数据合规");
        return Task.CompletedTask;
    }

    private Task OnInValidSubmit(EditContext context)
    {
        Trace1.Log("数据非法");
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        // TODO: 移动到数据库中
        new AttributeItem() {
            Name = nameof(PopConfirmButton.IsLink),
            Description = "是否为 A 标签渲染组件",
            Type = "bool",
            ValueList = "true/false",
            DefaultValue = "false"
        },
        new AttributeItem() {
            Name = "Text",
            Description = "显示标题",
            Type = "string",
            ValueList = "",
            DefaultValue = "删除"
        },
        new AttributeItem() {
            Name = "Icon",
            Description = "按钮图标",
            Type = "string",
            ValueList = "",
            DefaultValue = "fa fa-remove"
        },
        new AttributeItem() {
            Name = "CloseButtonText",
            Description = "关闭按钮显示文字",
            Type = "string",
            ValueList = "",
            DefaultValue = "关闭"
        },
        new AttributeItem() {
            Name = "CloseButtonColor",
            Description = "确认按钮颜色",
            Type = "Color",
            ValueList = "None / Active / Primary / Secondary / Success / Danger / Warning / Info / Light / Dark / Link",
            DefaultValue = "Secondary"
        },
        new AttributeItem() {
            Name = "Color",
            Description = "颜色",
            Type = "Color",
            ValueList = "None / Active / Primary / Secondary / Success / Danger / Warning / Info / Light / Dark / Link",
            DefaultValue = "None"
        },
        new AttributeItem() {
            Name = "ConfirmButtonText",
            Description = "确认按钮显示文字",
            Type = "string",
            ValueList = "",
            DefaultValue = "确定"
        },
        new AttributeItem() {
            Name = "ConfirmButtonColor",
            Description = "确认按钮颜色",
            Type = "None / Active / Primary / Secondary / Success / Danger / Warning / Info / Light / Dark / Link",
            ValueList = "",
            DefaultValue = "Primary"
        },
        new AttributeItem() {
            Name = "ConfirmIcon",
            Description = "确认框图标",
            Type = "string",
            ValueList = "",
            DefaultValue = "fa fa-exclamation-circle text-info"
        },
        new AttributeItem() {
            Name = "Content",
            Description = "显示文字",
            Type = "string",
            ValueList = "",
            DefaultValue = "确认删除吗？"
        },
        new AttributeItem() {
            Name = "Placement",
            Description = "位置",
            Type = "Placement",
            ValueList = "Auto / Top / Left / Bottom / Right",
            DefaultValue = "Auto"
        },
        new AttributeItem() {
            Name = "Title",
            Description = "Popover 弹窗标题",
            Type = "string",
            ValueList = "",
            DefaultValue = " "
        }
    };

    /// <summary>
    /// 获得事件方法
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<EventItem> GetEvents() => new EventItem[]
    {
        new EventItem()
        {
            Name = "OnConfirm",
            Description="点击确认时回调方法",
            Type ="Func<Task>"
        },
        new EventItem()
        {
            Name = "OnClose",
            Description="点击关闭时回调方法",
            Type ="Func<Task>"
        },
        new EventItem()
        {
            Name = "OnBeforeClick",
            Description="点击确认弹窗前回调方法 返回真时弹出弹窗 返回假时不弹出",
            Type ="Func<Task<bool>>"
        }
    };
}
