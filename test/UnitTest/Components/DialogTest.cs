﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared;

namespace UnitTest.Components;

public class DialogTest : DialogTestBase
{
    [Fact]
    public void Show_Ok()
    {
        #region Show
        var cut = Context.RenderComponent<BootstrapBlazorRoot>(pb =>
        {
            pb.AddChildContent<MockDialogTest>();
        });
        var dialog = cut.FindComponent<MockDialogTest>().Instance.DialogService;

        var closed = false;
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            BodyTemplate = builder => builder.AddContent(0, "Test-BodyTemplate"),
            HeaderTemplate = builder => builder.AddContent(0, "Test-HeaderTemplate"),
            FooterTemplate = builder => builder.AddContent(0, "Test-FooterTemplate"),
            Class = "test-class",
            ShowMaximizeButton = true,
            OnCloseAsync = () =>
            {
                closed = true;
                return Task.CompletedTask;
            }
        }));

        // 全屏按钮
        Assert.Contains("btn-maximize", cut.Markup);

        // 代码覆盖模板单元测试
        Assert.Contains("Test-BodyTemplate", cut.Markup);
        Assert.Contains("Test-HeaderTemplate", cut.Markup);
        Assert.Contains("Test-FooterTemplate", cut.Markup);
        Assert.Contains("test-class", cut.Markup);

        // 测试关闭逻辑
        var modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());
        Assert.True(closed);

        // 测试 Component 赋值逻辑
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            Component = BootstrapDynamicComponent.CreateComponent<Button>(),
            BodyTemplate = null
        }));
        Assert.Contains("class=\"btn btn-primary\"", cut.Markup);
        modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());

        // 测试 Component 与 BodyTemplate 均为 null 逻辑
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            Component = null,
            BodyTemplate = null
        }));
        cut.InvokeAsync(() => modal.Instance.Close());
        #endregion

        #region ShowSearchDialog
        // 无按钮回调赋值
        var option = new SearchDialogOption<Foo>()
        {
            Title = "Test-SearchDialogTitle",
            Model = new Foo(),
            ItemsPerRow = 2,
            RowType = RowType.Inline,
            LabelAlign = Alignment.Left,
            ResetButtonText = null,
            QueryButtonText = null,
            Items = null
        };
        cut.InvokeAsync(() => dialog.ShowSearchDialog(option));

        // 重置按钮委托为空 null
        var button = cut.FindComponents<Button>().First(b => b.Instance.Text == "重置");
        cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());

        // 搜索按钮委托为空
        cut.InvokeAsync(() => dialog.ShowSearchDialog(option));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "查询");
        cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());

        // 重置按钮
        var reset = false;
        option.OnResetSearchClick = () =>
        {
            reset = true;
            return Task.CompletedTask;
        };
        cut.InvokeAsync(() => dialog.ShowSearchDialog(option));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "重置");
        cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());
        Assert.True(reset);

        // 搜索按钮
        var search = false;
        option.DialogBodyTemplate = foo => builder => builder.AddContent(0, foo.Name);
        option.OnSearchClick = () =>
        {
            search = true;
            return Task.CompletedTask;
        };
        cut.InvokeAsync(() => dialog.ShowSearchDialog(option));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "查询");
        cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());
        Assert.True(search);
        #endregion

        #region ShowEditDialog
        // 无按钮回调赋值
        var editOption = new EditDialogOption<Foo>()
        {
            Model = new Foo(),
            ItemsPerRow = 2,
            RowType = RowType.Inline,
            ItemChangedType = ItemChangedType.Add,
            IsTracking = false,
            LabelAlign = Alignment.Left
        };
        cut.InvokeAsync(() => dialog.ShowEditDialog(editOption));
        cut.InvokeAsync(() => modal.Instance.Close());

        // 设置关闭回调
        closed = false;
        editOption.OnCloseAsync = () =>
        {
            closed = true;
            return Task.CompletedTask;
        };
        cut.InvokeAsync(() => dialog.ShowEditDialog(editOption));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "关闭");
        cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());
        Assert.True(closed);

        // 设置保存回调
        var saved = false;
        editOption.ShowLoading = true;
        editOption.OnEditAsync = context =>
        {
            saved = true;
            return Task.FromResult(true);
        };

        var model = new Foo() { Name = "Test" };
        var parameters = new Dictionary<string, object?>()
        {
            ["Field"] = "Name",
            ["FieldExpression"] = model.GenerateValueExpression()
        };
        var item = new EditorItem<Foo, string>();
        cut.InvokeAsync(() => item.SetParametersAsync(ParameterView.FromDictionary(parameters)));
        editOption.Items = new IEditorItem[]
        {
            item
        };
        editOption.Model = model;
        cut.InvokeAsync(() => dialog.ShowEditDialog(editOption));
        var form = cut.Find("form");
        form.Submit();
        Assert.True(saved);

        // 测试 DialogBodyTemplate
        editOption.DialogBodyTemplate = foo => builder => builder.AddContent(0, "test");
        cut.InvokeAsync(() => dialog.ShowEditDialog(editOption));
        form.Submit();

        // Modal is Null
        editOption.Model = null;
        Assert.ThrowsAsync<InvalidOperationException>(() => cut.InvokeAsync(() => dialog.ShowEditDialog(editOption)));
        cut.InvokeAsync(() => cut.Find(".btn-close").Click());
        #endregion

        #region ShowModal
        var result = false;
        var resultOption = new ResultDialogOption()
        {
            ComponentParamters = new Dictionary<string, object>()
            {
                [nameof(MockModalDialog.Value)] = result,
                [nameof(MockModalDialog.ValueChanged)] = EventCallback.Factory.Create<bool>(this, b => result = b)
            }
        };

        // 点击的是 Yes 按钮
        cut.InvokeAsync(() => dialog.ShowModal<MockModalDialog>(resultOption));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "确认");
        cut.InvokeAsync(() => button.Instance.OnClick.InvokeAsync());
        Assert.True(result);

        // 点击的是 No 按钮
        result = true;
        resultOption = new ResultDialogOption()
        {
            ComponentParamters = new Dictionary<string, object>()
            {
                [nameof(MockModalDialog.Value)] = result,
                [nameof(MockModalDialog.ValueChanged)] = EventCallback.Factory.Create<bool>(this, b => result = b)
            }
        };
        cut.InvokeAsync(() => dialog.ShowModal<MockModalDialog>(resultOption));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "取消");
        cut.InvokeAsync(() => button.Instance.OnClick.InvokeAsync());
        Assert.False(result);

        // 点击关闭按钮
        resultOption = new ResultDialogOption()
        {
            ShowCloseButton = true,
            ComponentParamters = new Dictionary<string, object>()
            {
                [nameof(MockModalDialog.Value)] = result,
                [nameof(MockModalDialog.ValueChanged)] = EventCallback.Factory.Create<bool>(this, b => result = b)
            },
            OnCloseAsync = () => Task.CompletedTask
        };
        cut.InvokeAsync(() => dialog.ShowModal<MockModalDialog>(resultOption));
        button = cut.FindComponents<Button>().First(b => b.Instance.Text == "关闭");
        cut.InvokeAsync(() => button.Instance.OnClick.InvokeAsync());
        #endregion

        #region 弹窗中的弹窗测试
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            // 弹窗中按钮
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<Button>(new Dictionary<string, object?>()
            {
                [nameof(Button.OnClickWithoutRender)] = () =>
                {
                    // 继续弹窗
                    dialog.Show(new DialogOption()
                    {
                        BodyTemplate = builder =>
                        {
                            builder.AddContent(0, "Test");
                        }
                    });
                    return Task.CompletedTask;
                }
            }).Render()
        }));

        // 弹出第二个弹窗
        var buttonInDialog = cut.Find(".btn-primary");
        buttonInDialog.Click();
        Assert.Equal(2, cut.FindComponents<ModalDialog>().Count);

        // 关闭第二个弹窗
        var btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        Assert.Equal(1, cut.FindComponents<ModalDialog>().Count);

        // 关闭第一个弹窗
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        Assert.Equal(0, cut.FindComponents<ModalDialog>().Count);
        #endregion

        #region 全屏弹窗
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            FullScreenSize = FullScreenSize.Large
        }));
        Assert.Contains("modal-fullscreen-lg-down", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        #endregion

        #region IsCenter
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            IsCentered = true
        }));
        Assert.Contains("modal-dialog-centered", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();

        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            IsCentered = false
        }));
        Assert.DoesNotContain("modal-dialog-centered", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        #endregion

        #region IsKeyboard
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            IsKeyboard = true
        }));
        Assert.Contains("data-bs-keyboard=\"true\"", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();

        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            IsKeyboard = false
        }));
        Assert.DoesNotContain("data-bs-keyboard\"false\"", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        #endregion

        #region ShowHeaderCloseButton
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowHeaderCloseButton = true
        }));
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();

        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowHeaderCloseButton = false
        }));
        Assert.DoesNotContain("btn-close", cut.Markup);
        btnClose = cut.FindAll(".btn-secondary")[cut.FindAll(".btn-secondary").Count - 1];
        btnClose.Click();
        #endregion

        #region ShowPrintButton
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowPrintButton = true
        }));
        Assert.Contains("btn-print", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();

        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowPrintButton = false
        }));
        Assert.DoesNotContain("btn-print", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();

        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowPrintButton = true,
            ShowPrintButtonInHeader = true,
            PrintButtonText = "Print-Test"
        }));
        Assert.Contains("btn-print", cut.Markup);
        Assert.Contains("Print-Test", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        #endregion

        #region ShowSaveButton
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowSaveButton = true,
            SaveButtonText = "Save-Test",
            CloseButtonText = "Close-Test",
            BodyContext = "Test"
        }));
        Assert.Contains("Save-Test", cut.Markup);
        Assert.Contains("Close-Test", cut.Markup);
        btnClose = cut.FindAll(".btn-close")[cut.FindAll(".btn-close").Count - 1];
        btnClose.Click();
        #endregion

        #region OnSaveAsync
        var save = false;
        cut.InvokeAsync(() => dialog.Show(new DialogOption()
        {
            ShowSaveButton = true,
            IsAutoCloseAfterSave = true,
            IsDraggable = false,
            OnSaveAsync = () =>
            {
                save = true;
                return Task.FromResult(save);
            }
        }));
        btnClose = cut.FindAll(".btn-primary")[cut.FindAll(".btn-primary").Count - 1];
        btnClose.Click();
        Assert.True(save);
        #endregion

        #region ShowSaveDialog
        cut.InvokeAsync(() => dialog.ShowSaveDialog<MockDialogTest>("Title", () => Task.FromResult(true), p => { }, op => op.Class = "test"));
        modal.FindAll("button")[modal.FindAll("button").Count - 1].Click();
        cut.InvokeAsync(() => dialog.ShowSaveDialog<MockDialogTest>("Title"));
        modal.FindAll("button")[modal.FindAll("button").Count - 1].Click();
        #endregion

        #region ShowValidateFormDialog
        cut.InvokeAsync(() => dialog.ShowValidateFormDialog<MockValidateFormDialog>("ValidateFormDialog"));
        var btn = cut.Find(".btn-close");
        cut.InvokeAsync(() => btn.Click());

        Func<DialogOption, Dictionary<string, object?>> parameterFactory = op => new Dictionary<string, object?>();
        Action<DialogOption> configureOption = op => op.Class = "ValidateFormDialog-Class";
        cut.InvokeAsync(() => dialog.ShowValidateFormDialog<MockValidateFormDialog>("ValidateFormDialog", parameterFactory, configureOption));
        btn = cut.Find(".btn-close");
        cut.InvokeAsync(() => btn.Click());
        #endregion

        #region ShowCloseDialog
        Action<Dictionary<string, object?>> closeDialogParameterFactory = op => new Dictionary<string, object?>();
        cut.InvokeAsync(() => dialog.ShowCloseDialog<MockValidateFormDialog>("CloseDialog", closeDialogParameterFactory, configureOption));
        btn = cut.Find(".btn-close");
        cut.InvokeAsync(() => btn.Click());
        cut.InvokeAsync(() => dialog.ShowCloseDialog<MockValidateFormDialog>("CloseDialog"));
        btn = cut.Find(".btn-close");
        cut.InvokeAsync(() => btn.Click());
        #endregion
    }

    private class MockValidateFormDialog : ComponentBase
    {

    }

    private class MockDialogTest : ComponentBase
    {
        [Inject]
        [NotNull]
        public DialogService? DialogService { get; set; }
    }

    private class MockModalDialog : ComponentBase, IResultDialog
    {
        [Parameter]
        public bool Value { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        public async Task OnClose(DialogResult result)
        {
            if (result == DialogResult.Yes)
            {
                Value = true;
                if (ValueChanged.HasDelegate)
                {
                    await ValueChanged.InvokeAsync(Value);
                }
            }
            else
            {
                Value = false;
                if (ValueChanged.HasDelegate)
                {
                    await ValueChanged.InvokeAsync(Value);
                }
            }
        }
    }
}
