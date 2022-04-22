// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared.Common;

namespace BootstrapBlazor.Shared.Samples;

public partial class ComponentReconnectModals
{
    private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        new AttributeItem()
        {
            Name = "BackdropBackgroundColor",
            Description = Localizer["Event1"],
            Type = "string",
            ValueList = "",
            DefaultValue = "rgba(0, 0, 0, .75)"
        },
        new AttributeItem()
        {
            Name = "ConnectionStateBackgroundColor",
            Description = Localizer["Event2"],
            Type = "string",
            ValueList = "",
            DefaultValue = "rgba(255, 255, 255, .8)"
        },
        new AttributeItem()
        {
            Name = "ConnectionStatePadding",
            Description = Localizer["Event3"],
            Type = "string",
            ValueList = "",
            DefaultValue = "64px"
        },
        new AttributeItem()
        {
            Name = "ReconnectingTemplate",
            Description = Localizer["Event4"],
            Type = "RenderFragment",
            ValueList = "",
            DefaultValue = ""
        },
        new AttributeItem()
        {
            Name = "ReconnectFailedTemplate",
            Description = Localizer["Event5"],
            Type = "RenderFragment",
            ValueList = "",
            DefaultValue = ""
        },
        new AttributeItem()
        {
            Name = "ReconnectRejectedTemplate",
            Description = Localizer["Event6"],
            Type = "RenderFragment",
            ValueList = "",
            DefaultValue = ""
        },
    };
}
