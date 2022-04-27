// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using BootstrapBlazor.Shared.Components;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// 屏幕键盘
/// </summary>
public sealed partial class OnScreenKeyboards
{
    string BindValue = "virtualkeyboard";
    string ClassName = "virtualkeyboard";
    string ClassName1 = "virtualkeyboard1";
    string ClassName2 = "virtualkeyboard2";
    string ClassName3 = "virtualkeyboard3";

    static Dictionary<string, string> keys1 = new Dictionary<string, string>() { { "0", "L" }, { "1", "O" } };
    static Dictionary<string, string> keys2 = new Dictionary<string, string>() { { "0", "V" }, { "1", "E" } };
    static List<Dictionary<string, string>> keysArray = new List<Dictionary<string, string>>() { keys1, keys2 };

    KeyboardOption Option1 = new KeyboardOption()
    {
        //keysArrayOfObjects = keysArray,
        keysFontFamily = "Barlow",
        keysFontWeight = "500",
        Theme = KeyboardTheme.dark,
    };
    KeyboardOption Option2 = new KeyboardOption()
    {
        KeyboardSpecialcharacters = KeyboardSpecialcharacters.europe
    };
    KeyboardOption Option3 = new KeyboardOption()
    {
        CustomerKeyboardSpecialcharacters = new string[] { "中", "国", "女", "足", "牛啊" }
    };


    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        // TODO: 移动到数据库中
        new AttributeItem()
        {
            Name = "OnResult",
            Description = "签名结果回调方法",
            Type = "Func<string, Task>?",
            ValueList = " — ",
            DefaultValue = " — "
        },
    };
}
