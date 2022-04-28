﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.JSInterop;

namespace BootstrapBlazor.Components;

/// <summary>
/// JSInterop 类
/// </summary>
public class JSInterop<TValue> : IDisposable where TValue : class
{
    private readonly IJSRuntime _jsRuntime;
    private DotNetObjectReference<TValue>? _objRef;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jsRuntime"></param>
    public JSInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Invoke 方法
    /// </summary>
    /// <param name="value"></param>
    /// <param name="el"></param>
    /// <param name="func"></param>
    /// <param name="args"></param>
    public async ValueTask InvokeVoidAsync(TValue value, object? el, string func, params object[] args)
    {
        _objRef = DotNetObjectReference.Create(value);
        var paras = new List<object>()
        {
            _objRef
        };
        paras.AddRange(args);
        await _jsRuntime.InvokeVoidAsync(el, func, paras.ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="el"></param>
    /// <param name="func"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public async ValueTask<TReturn?> InvokeAsync<TReturn>(TValue value, object? el, string func, params object[] args)
    {
        _objRef = DotNetObjectReference.Create(value);
        var paras = new List<object>()
        {
            _objRef
        };
        paras.AddRange(args);
        return await _jsRuntime.InvokeAsync<TReturn>(el, func, paras.ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal ValueTask<bool> GetGeolocationItemAsync(TValue value, string callbackMethodName)
    {
        _objRef = DotNetObjectReference.Create(value);
        return _jsRuntime.InvokeAsync<bool>("$.bb_geo_getCurrnetPosition", _objRef, callbackMethodName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal ValueTask<long> GetWatchPositionItemAsync(TValue value, string callbackMethodName)
    {
        _objRef = DotNetObjectReference.Create(value);
        return _jsRuntime.InvokeAsync<long>("$.bb_geo_watchPosition", _objRef, callbackMethodName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal ValueTask<bool> SetClearWatchPositionAsync(long watchid)
    {
        return _jsRuntime.InvokeAsync<bool>("$.bb_geo_clearWatchLocation", watchid);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal ValueTask CheckNotifyPermissionAsync(TValue value, string? callbackMethodName = null, bool requestPermission = true)
    {
        _objRef = DotNetObjectReference.Create(value);
        return _jsRuntime.InvokeVoidAsync("$.bb_notify_checkPermission", _objRef, callbackMethodName ?? "", requestPermission);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal ValueTask<bool> Dispatch(TValue value, NotificationItem model, string? callbackMethodName = null)
    {
        _objRef = DotNetObjectReference.Create(value);
        return _jsRuntime.InvokeAsync<bool>("$.bb_notify_display", _objRef, callbackMethodName, model);
    }

    /// <summary>
    /// Dispose 方法
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_objRef != null)
            {
                _objRef.Dispose();
                _objRef = null;
            }
        }
    }

    /// <summary>
    /// Dispose 方法
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
