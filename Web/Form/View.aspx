<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Site.Form.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text=''></asp:Label></li>
        </ul>
        <asp:PlaceHolder runat="server" ID="phRespCollab" Visible="false">
            <div class="row">
                <strong><asp:Literal ID="Literal4" runat="server" Text='<%$Resources: Message, responsable %>'></asp:Literal>:&nbsp;</strong><asp:Label ID="lblResponsable" runat="server" Text="-"></asp:Label>
            </div>
            <div class="row">
                <strong><asp:Literal ID="Literal5" runat="server" Text='<%$Resources: Message, collaborators %>'></asp:Literal>:&nbsp;</strong><asp:Label ID="lblCollaborator" runat="server" Text="-"></asp:Label>
            </div>
        </asp:PlaceHolder>
        <div style="width: 100%">
            <div id="chart_div" style="width: 450px; height: 250px; float: left"></div>
            <div id="piechart" style="width: 450px; height: 250px; float: left"></div>
        </div>
        <asp:HiddenField ID="hdnResponseId" runat="server" Value="0" />
        <asp:Repeater runat="server" ID="rptBlocks" OnItemDataBound="rptBlocks_ItemDataBound">
            <HeaderTemplate>
                <ol class="ol-view">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:Label ID="Label5" Text='<%# Eval("Name") %>' runat="server" CssClass="title" />
                    <asp:Repeater runat="server" ID="rptSubBlocks" OnItemDataBound="rptSubBlocks_ItemDataBound">
                        <ItemTemplate>
                            <br />
                            <asp:Label ID="Label5" Text='<%# Eval("Name") %>' runat="server" CssClass="subtitle" />
                            <asp:Repeater runat="server" ID="rptQuestions">
                                <HeaderTemplate>
                                    <table class="styler_table">
                                        <thead class="styler_bg_color">
                                            <tr>
                                                <th>
                                                    <asp:Literal ID="litIndex" runat="server" Text='<%$Resources: Message, index %>'></asp:Literal>
                                                </th>
                                                <th>
                                                    <asp:Literal ID="Literal1" runat="server" Text='<%$Resources: Message, question %>'></asp:Literal>
                                                </th>
                                                <th>
                                                    <asp:Literal ID="Literal2" runat="server" Text='<%$Resources: Message, score %>'></asp:Literal>
                                                </th>
                                                <th>
                                                    <asp:Literal ID="Literal3" runat="server" Text='<%$Resources: Message, observation %>'></asp:Literal>
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="block" id='<%# Eval("Id") %>'>
                                        <td>
                                            <asp:Label ID="Label3" Text='<%# Eval("BaseQuestion.Index") %>' runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" Text='<%# Eval("BaseQuestion.Question") %>' runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" Text='<%# Eval("Score") == null ? "N/A" : Eval("Score") %>' runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Text='<%# Eval("Observation") %>' runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ol>
            </FooterTemplate>
        </asp:Repeater>
        <a href="#" style="float: left" onclick="openTrackingModal(); return false;">
            <asp:Literal ID="litIndex" runat="server" Text='<%$Resources: Message, calculate_tracking %>'></asp:Literal></a>
        <div id="div_tracking" style="display: none;">
            <asp:Literal ID="litTracking" runat="server"></asp:Literal>
        </div>
    </div>
    <script type="text/javascript">
        function showQuestions(id) {
            $('.block-questions:not(#block_' + id).hide();
            $('#block_' + id).toggle();
        }

        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawCharts);

        function drawCharts() {
            $.ajax({
                url: "/Handlers/Chart/ChartData.ashx",
                type: 'POST',
                data: { t: 'score_per_block', rfid: $('#<%= hdnResponseId.ClientID %>').val() },
                cache: false,
                success: function (json) {
                    //Monta o array de resultados, (Coluna 1 = Blocos e coluna 2 = pontuação)
                    var drawData = [];
                    drawData.push(['Blocos', 'Pontuação', { role: 'annotation' }, { role: 'tooltip' }]);

                    for (var i in json) {
                        drawData.push([json[i].ColumnName, json[i].Percentage, json[i].Label, json[i].TotalScore + "/" + json[i].MaxScore]);
                    }

                    var data = new google.visualization.arrayToDataTable(drawData);

                    var options = {
                        title: '',
                        legend: { position: "none" },
                        vAxis: { format: '#%' }
                    };

                    var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
                    chart.draw(data, options);
                }
            });

            $.ajax({
                url: "/Handlers/Chart/ChartData.ashx",
                type: 'POST',
                data: { t: 'score_per_form', rfid: $('#<%= hdnResponseId.ClientID %>').val() },
                cache: false,
                success: function (json) {
                    //Monta o array de resultados, (Coluna 1 = Blocos e coluna 2 = pontuação)
                    var drawData = [];
                    drawData.push(['Blocos', 'Pontuação']);

                    for (var i in json) {
                        drawData.push([json[i].ColumnName, json[i].TotalScore]);
                    }

                    var data = new google.visualization.arrayToDataTable(drawData);

                    var options = {
                        title: '',
                        forceIFrame: true,
                        titleTextStyle: { color: 'black' }
                    };

                    var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                    chart.draw(data, options);
                }
            });
        }

        function openTrackingModal() {
            $("#div_tracking").toggle();

            $("html, body").animate({ scrollTop: $(window).scrollTop() + $(window).height() }, 1000);
        }
    </script>
</asp:Content>
