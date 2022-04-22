// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
///
/// </summary>
public partial class ComponentReconnectModal
{
    /// <summary>
    /// 获得/设置 模态对话框的背景色 默认值为 rgba(0, 0, 0, .75)
    /// </summary>
    [Parameter]
    public string BackdropBackgroundColor { get; set; } = "rgba(0, 0, 0, .75)";

    /// <summary>
    /// 获得/设置 连接显示状态对话框的背景色 默认值为 rgba(255, 255, 255, .8)
    /// </summary>
    [Parameter]
    public string ConnectionStateBackgroundColor { get; set; } = "rgba(255, 255, 255, .8)";

    /// <summary>
    /// 获得/设置 连接状态对话框的padding 默认值为 64px
    /// </summary>
    [Parameter]
    public string ConnectionStatePadding { get; set; } = "64px";

    /// <summary>
    /// 获得/设置 正在尝试重试连接对话框的模板
    /// </summary>
    [Parameter]
    public RenderFragment? ReconnectingTemplate { get; set; }

    /// <summary>
    /// 获得/设置 连接失败对话框的模板
    /// </summary>
    [Parameter]
    public RenderFragment? ReconnectFailedTemplate { get; set; }

    /// <summary>
    /// 获得/设置 连接被拒绝对话框的模板
    /// </summary>
    [Parameter]
    public RenderFragment? ReconnectRejectedTemplate { get; set; }
}
