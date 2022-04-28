﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace BootstrapBlazor.Components;

/// <summary>
/// Upload 组件基类
/// </summary>
public abstract class UploadBase<TValue> : ValidateBase<TValue>, IUpload
{
    /// <summary>
    /// 获得 组件样式
    /// </summary>
    protected string? ClassString => CssBuilder.Default("upload")
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// 获得/设置 Upload 组件实例
    /// </summary>
    protected ElementReference UploaderElement { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected UploadFile? CurrentFile { get; set; }

    /// <summary>
    /// 获得/设置 上传文件集合
    /// </summary>
    protected List<UploadFile> UploadFiles { get; } = new List<UploadFile>();

    List<UploadFile> IUpload.UploadFiles { get => UploadFiles; }

    /// <summary>
    /// 获得/设置 上传接收的文件格式 默认为 null 接收任意格式
    /// </summary>
    [Parameter]
    public string? Accept { get; set; }

    /// <summary>
    /// 获得/设置 点击删除按钮时回调此方法
    /// </summary>
    [Parameter]
    public Func<UploadFile, Task<bool>>? OnDelete { get; set; }

    /// <summary>
    /// 获得/设置 点击浏览按钮时回调此方法
    /// </summary>
    [Parameter]
    public Func<UploadFile, Task>? OnChange { get; set; }

    private JSModule? Module { get; set; }

    /// <summary>
    /// OnAfterRender 方法
    /// </summary>
    /// <param name="firstRender"></param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && !IsDisabled)
        {
            // support drag
            Module = await JSRuntime.LoadModule("upload.js");
            await Module.InvokeVoidAsync("bb_upload_drag_init", UploaderElement, InputFileId);
        }
    }

    /// <summary>
    /// 获得 InputFile 组件 Id 方法
    /// </summary>
    /// <returns></returns>
    protected string InputFileId => $"{Id}_InputFile";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected string? GetFileName(UploadFile? item) => item?.OriginFileName ?? item?.FileName ?? Value?.ToString();

    /// <summary>
    /// 显示/隐藏验证结果方法
    /// </summary>
    /// <param name="results"></param>
    /// <param name="validProperty">是否对本属性进行数据验证</param>
    public override void ToggleMessage(IEnumerable<ValidationResult> results, bool validProperty)
    {
        if (FieldIdentifier != null)
        {
            var messages = results.Where(item => item.MemberNames.Any(m => UploadFiles.Any(f => f.ValidateId?.Equals(m, StringComparison.OrdinalIgnoreCase) ?? false)));
            if (messages.Any())
            {
                IsValid = false;
                if (CurrentFile != null)
                {
                    var msg = messages.FirstOrDefault(m => m.MemberNames.Any(f => f.Equals(CurrentFile.ValidateId, StringComparison.OrdinalIgnoreCase)));
                    if (msg != null)
                    {
                        ErrorMessage = msg.ErrorMessage;
                        TooltipMethod = validProperty ? "show" : "enable";
                    }
                }
            }
            else
            {
                ErrorMessage = null;
                IsValid = true;
                TooltipMethod = "dispose";
            }
            OnValidate(IsValid);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected virtual async Task<bool> OnFileDelete(UploadFile item)
    {
        var ret = true;
        if (OnDelete != null)
        {
            ret = await OnDelete(item);
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected virtual Task OnFileChange(InputFileChangeEventArgs args)
    {
        // 判定可为空
        var type = NullableUnderlyingType ?? typeof(TValue);
        if (type.IsAssignableTo(typeof(IBrowserFile)))
        {
            CurrentValue = (TValue)args.File;
        }
        if (type.IsAssignableTo(typeof(List<IBrowserFile>)))
        {
            CurrentValue = (TValue)(object)UploadFiles.Select(f => f.File).ToList();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual IDictionary<string, object> GetUploadAdditionalAttributes()
    {
        var ret = new Dictionary<string, object>
        {
            { "hidden", "hidden" }
        };
        if (!string.IsNullOrEmpty(Accept))
        {
            ret.Add("accept", Accept);
        }
        ret.Add("id", InputFileId);
        return ret;
    }

    /// <summary>
    /// 清空上传列表方法
    /// </summary>
    public virtual void Reset()
    {
        UploadFiles.Clear();
        StateHasChanged();
    }

    /// <summary>
    /// DisposeAsyncCore 方法
    /// </summary>
    /// <param name="disposing"></param>
    /// <returns></returns>
    protected override async ValueTask DisposeAsyncCore(bool disposing)
    {
        await base.DisposeAsyncCore(disposing);

        if (disposing)
        {
            if (Module != null)
            {
                await Module.DisposeAsync();
            }
        }
    }
}
