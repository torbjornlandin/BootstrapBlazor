// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared.Common;

namespace BootstrapBlazor.Shared.Samples;

public partial class Reconnectors
{
    private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
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
