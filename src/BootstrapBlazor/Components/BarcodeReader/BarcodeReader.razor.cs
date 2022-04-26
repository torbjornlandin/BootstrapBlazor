﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// BarcodeReader 条码扫描
/// </summary>
public partial class BarcodeReader : IAsyncDisposable
{
    private JSModule<BarcodeReader>? Module { get; set; }

    private string AutoStopString => AutoStop ? "true" : "false";

    private bool IsDisabled { get; set; } = true;

    /// <summary>
    /// 获得/设置 扫描按钮文字 默认为 扫描
    /// </summary>
    [Parameter]
    [NotNull]
    public string? ButtonScanText { get; set; }

    /// <summary>
    /// 获得/设置 关闭按钮文字 默认为 关闭
    /// </summary>
    [Parameter]
    [NotNull]
    public string? ButtonStopText { get; set; }

    /// <summary>
    /// 获得/设置 自动关闭文字 默认为 自动关闭
    /// </summary>
    [Parameter]
    [NotNull]
    public string? AutoStopText { get; set; }

    /// <summary>
    /// 获得/设置 设备列表前置标签文字 默认为 摄像头
    /// </summary>
    [Parameter]
    [NotNull]
    public string? DeviceLabel { get; set; }

    /// <summary>
    /// 获得/设置 初始化设备列表文字 默认为 正在识别摄像头
    /// </summary>
    [Parameter]
    [NotNull]
    public string? InitDevicesString { get; set; }

    /// <summary>
    /// 获得/设置 未找到视频相关设备文字 默认为 未找到视频相关设备
    /// </summary>
    [Parameter]
    [NotNull]
    public string? NotFoundDevicesString { get; set; }

    /// <summary>
    /// 获得/设置 扫描方式 默认 Camera 从摄像头进行条码扫描
    /// </summary>
    [Parameter]
    public ScanType ScanType { get; set; }

    /// <summary>
    /// 获得/设置 摄像头设备切换回调方法 默认 null
    /// </summary>
    [Parameter]
    public Func<DeviceItem, Task>? OnDeviceChanged { get; set; }

    /// <summary>
    /// 获得/设置 初始化摄像头回调方法
    /// </summary>
    [Parameter]
    public Func<IEnumerable<DeviceItem>, Task>? OnInit { get; set; }

    /// <summary>
    /// 获得/设置 扫码结果回调方法
    /// </summary>
    [Parameter]
    public Func<string, Task>? OnResult { get; set; }

    /// <summary>
    /// 获得/设置 自动开启摄像头 默认为 false
    /// </summary>
    [Parameter]
    public bool AutoStart { get; set; }

    /// <summary>
    /// 获得/设置 扫描条码后自动关闭
    /// </summary>
    [Parameter]
    public bool AutoStop { get; set; }

    /// <summary>
    /// 获得/设置 扫码出错回调方法
    /// </summary>
    [Parameter]
    public Func<string, Task>? OnError { get; set; }

    /// <summary>
    /// 获得/设置 开始扫码回调方法
    /// </summary>
    [Parameter]
    public Func<Task>? OnStart { get; set; }

    /// <summary>
    /// 获得/设置 关闭扫码回调方法
    /// </summary>
    [Parameter]
    public Func<Task>? OnClose { get; set; }

    private ElementReference ScannerElement { get; set; }

    private IEnumerable<SelectedItem> Devices { get; set; } = Enumerable.Empty<SelectedItem>();

    [Inject]
    [NotNull]
    private IStringLocalizer<BarcodeReader>? Localizer { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        ButtonScanText ??= Localizer[nameof(ButtonScanText)];
        ButtonStopText ??= Localizer[nameof(ButtonStopText)];
        AutoStopText ??= Localizer[nameof(AutoStopText)];
        DeviceLabel ??= Localizer[nameof(DeviceLabel)];
        InitDevicesString ??= Localizer[nameof(InitDevicesString)];
        NotFoundDevicesString ??= Localizer[nameof(NotFoundDevicesString)];
    }

    /// <summary>
    /// OnAfterRenderAsync 方法
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.LoadModule("barcodereader.bundle.js", this);
            await Module.InvokeVoidAsync("bb_barcode", ScannerElement, "init", AutoStart);
        }
    }

    /// <summary>
    /// 初始化设备方法
    /// </summary>
    /// <param name="devices"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task InitDevices(IEnumerable<DeviceItem> devices)
    {
        Devices = devices.Select(i => new SelectedItem { Value = i.DeviceId, Text = i.Label });
        IsDisabled = !Devices.Any();

        if (OnInit != null) await OnInit(devices);
        if (IsDisabled) InitDevicesString = NotFoundDevicesString;
        StateHasChanged();
    }

    /// <summary>
    /// 扫描完成回调方法
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task GetResult(string val)
    {
        if (OnResult != null) await OnResult.Invoke(val);
    }

    /// <summary>
    /// 扫描发生错误回调方法
    /// </summary>
    /// <param name="err"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task GetError(string err)
    {
        if (OnError != null) await OnError.Invoke(err);
    }

    /// <summary>
    /// 开始扫描回调方法
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public async Task Start()
    {
        if (OnStart != null) await OnStart.Invoke();
    }

    /// <summary>
    /// 停止扫描回调方法
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public async Task Stop()
    {
        if (OnClose != null) await OnClose.Invoke();
    }

    private async Task OnSelectedItemChanged(SelectedItem item)
    {
        if (OnDeviceChanged != null)
        {
            await OnDeviceChanged(new DeviceItem() { DeviceId = item.Value, Label = item.Text });
        }
    }

    /// <summary>
    /// DisposeAsyncCore 方法
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual async ValueTask DisposeAsyncCore(bool disposing)
    {
        if (disposing)
        {
            if (Module != null)
            {
                await Module.InvokeVoidAsync("bb_barcode_dispose");
                await Module.DisposeAsync();
            }
        }
    }

    /// <summary>
    /// DisposeAsync 方法
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore(true);
        GC.SuppressFinalize(this);
    }
}
