<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WTFChannel.DefaultPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>WTF Channel Do I Watch?</title>
        <link href="styles.css" rel="stylesheet" type="text/css" />
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#ServiceProviders').change(function () {
                    createCookie("SelectedServiceId", $(this).val());
                })
            });

            function createCookie(name, value, days) {
                if (days) {
                    var date = new Date();
                    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                    var expires = "; expires=" + date.toGMTString();
                }
                else var expires = "";
                document.cookie = name + "=" + value + expires + "; path=/";
            }
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="input">
                <table>
                    <tr>
                        <td class="alignRight"><asp:Label ID="LabelApiKey" runat="server" Text="Rovi TV Listings API Key:" AssociatedControlID="ApiKey"></asp:Label></td>
                        <td><asp:TextBox ID="ApiKey" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="alignRight"><asp:Label ID="LabelCountryCode" runat="server" Text="Country Code:" AssociatedControlID="CountryCode"></asp:Label></td>
                        <td><asp:TextBox ID="CountryCode" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="alignRight"><asp:Label ID="LabelPostalCode" runat="server" Text="Postal Code:" AssociatedControlID="PostalCode"></asp:Label></td>
                        <td><asp:TextBox ID="PostalCode" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="GetServiceProviders" runat="server" Text="Get Service Providers" OnClick="GetServiceProviders_Click" UseSubmitBehavior="false"/><br />
                            <asp:Label ID="GetServiceProvidersError" runat="server" Text="Label" Visible="false" CssClass="error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="ServiceProvidersPanel" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td class="alignRight"><asp:Label ID="LabelServiceProviders" runat="server" Text="Service Providers:" AssociatedControlID="ServiceProviders"></asp:Label></td>
                            <td><asp:DropDownList ID="ServiceProviders" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="alignRight"><asp:Label ID="ShowNameLabel" runat="server" Text="Show Name:" AssociatedControlID="ShowName"></asp:Label></td>
                            <td><asp:TextBox ID="ShowName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="alignRight"><img src="rovi.png" /></td>
                            <td>
                                <asp:Button ID="FindChannel" runat="server" Text="WTF Channel Do I Watch?" OnClick="FindChannel_Click" /><br />
                                <asp:Label ID="FindChannelError" runat="server" Text="Label" Visible="false" CssClass="error"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:Panel ID="ResultsPanel" runat="server" Visible="false">
                <asp:Table ID="ResultsTable" runat="server">
                    <asp:TableHeaderRow ID="TableHeaderRow1" runat="server">
                        <asp:TableHeaderCell ID="TableHeaderCell1" runat="server">#</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="TableHeaderCell2" runat="server">Channel</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="TableHeaderCell3" runat="server">Time</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="TableHeaderCell4" runat="server">Name</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </asp:Panel>
        </form>
    </body>
</html>
