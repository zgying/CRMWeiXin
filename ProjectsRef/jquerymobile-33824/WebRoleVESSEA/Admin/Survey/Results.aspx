<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="Results.aspx.cs" Inherits="Web.Admin.Survey.Results" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
            AllowPaging="True" CellSpacing="0" GridLines="None">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting CellSelectionMode="None"></Selecting>
                <%--<Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                    ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />--%>
            </ClientSettings>
            <MasterTableView>
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                </EditFormSettings>
            </MasterTableView>
            <PagerStyle Position="TopAndBottom" />
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </telerik:RadGrid>
    </div>
    <div>
        <asp:Button ID="btnExport" Text="CSV Export" runat="server" OnClick="btnExport_Click">
        </asp:Button>
    </div>
</asp:Content>
