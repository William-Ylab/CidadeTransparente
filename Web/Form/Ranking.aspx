<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Ranking.aspx.cs" Inherits="Site.Form.Ranking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdLastPeriodId" runat="server" />
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='<%$Resources: Message, sent_questions %>'></asp:Label></li>
        </ul>
        <div id="div-legend" style="width: 940px; background-color: #fff">
            <div class="col-md-6">
            <b><asp:Literal ID="Literal2" Text='<%$Resources: Message, filter %>' runat="server" /></b>
            <asp:DropDownList ID="ddlState" onchange="filter(true); return false;" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-6">
                <asp:PlaceHolder runat="server" ID="phLastForm" Visible="false">
                        <button id="btnLastForm" runat="server" class="btn btn-primary" onserverclick="btnLastForm_ServerClick"><i class="fa fa-pencil-square-o"></i><asp:Literal ID="Literal1" Text='<%$Resources: Message, answer_form_and_compare %>' runat="server" /></button>
                </asp:PlaceHolder>
            </div>
            <br />
            <br />
            <div class="col-md-12">
                <b style="float: left"><asp:Literal ID="Literal3" Text='<%$Resources: Message, legend %>' runat="server" /></b>
                <ul style="list-style: none; float: left">
                <li style="display: inline-block;">
                    <div class=" mini-square square" style="background-color: #d94c4b; display: inline-block;"></div>
                    <asp:Literal ID="Literal4" Text='<%$Resources: Message, from_0_to_20 %>' runat="server" /></li>
                <li style="display: inline-block;">
                    <div class="mini-square square" style="background-color: #d56558; display: inline-block;"></div>
                    <asp:Literal ID="Literal5" Text='<%$Resources: Message, from_20_to_40 %>' runat="server" /></li>
                <li style="display: inline-block;">
                    <div class="mini-square square" style="background-color: #d7824c; display: inline-block;"></div>
                    <asp:Literal ID="Literal6" Text='<%$Resources: Message, from_40_to_60 %>' runat="server" /></li>
                <li style="display: inline-block;">
                    <div class="mini-square square" style="background-color: #eda637; display: inline-block;"></div>
                    <asp:Literal ID="Literal7" Text='<%$Resources: Message, from_60_to_80 %>' runat="server" /></li>
                <li style="display: inline-block;">
                    <div class="mini-square square" style="background-color: #86aa65; display: inline-block;"></div>
                    <asp:Literal ID="Literal8" Text='<%$Resources: Message, from_80_to_100 %>' runat="server" /></li>
                <li style="display: inline-block;">
                    <div class="mini-square square" style="background-color: #8aabaf; display: inline-block;"></div>
                    <asp:Literal ID="Literal9" Text='<%$Resources: Message, from_NA_to_NA %>' runat="server" /></li>
            </ul>
            </div>
            <div id="div-header" class="div_table_header" style="display: none;">
            </div>
        </div>
        <table id="tblRanking" style="border-collapse: separate; border-spacing: 3px;" class="styler_table">
            <thead class="styler_bg_color"></thead>
            <tbody></tbody>
        </table>
        <div style="height: 300px;"></div>
    </div>

    <%--<a href="#" id="example" class="btn btn-success" rel="popover" data-content="It's so simple to create a tooltop for my website!" data-original-title="Twitter Bootstrap Popover">hover for popover</a>--%>
    <script type="text/javascript">

        $(document).ready(function () {
            getRankings();
            var pos = $('#tblRanking thead').offset();

            $(window).scroll(function () {
                if (parseInt($(this).scrollTop()) > (pos.top - 98)) {
                    $('#div-header').css('display', 'block');
                    $('#div-legend').css('top', '98px');
                    $('#div-legend').css('position', 'fixed');
                    $('#div-header thead tr th:first').css('width', '176px');
                } else {
                    $('#div-header').css('display', 'none');
                    $('#div-legend').css('position', '');
                }
            });

            //$("#example").popover();
        });
        var arrayData = [];
        var arrayDataFiltered = [];
        var propSort = '';
        var propAsc = '';

        function getRankings() {
            $.ajax({
                url: "/Handlers/Ranking/Get.ashx",
                type: 'POST',
                cache: false,
                success: function (data) {
                    arrayData = data;
                    arrayDataFiltered = data;

                    $('#tmpltblranking_head').tmpl(data[0]).appendTo('#tblRanking thead');
                    $('#div-header').append($('#tblRanking').remove('tbody')[0].outerHTML.replace('<tbody>', '').replace('</tbody'));

                    fillRanking(data);
                }
            });

            return false;
        }

        function filter(fill) {
            var stateId = $('#<%= ddlState.ClientID%>').val();

            arrayDataFiltered = [];

            if (stateId != '') {
                $(arrayData).each(function (i, v) {
                    if (v.State == stateId) {
                        arrayDataFiltered.push(v);
                    }
                });
            } else {
                arrayDataFiltered = arrayData;
            }

            if (fill == true) {
                fillRanking(arrayDataFiltered);
            } else {
                $('.div_popover').popover();
            }
        }

        function sortResults(prop, asc, imgid) {
            if (propSort == prop) {
                if (propAsc == true) {
                    asc = false;
                } else {
                    asc = true;
                }
            } else {
                asc = true;
            }

            propSort = prop;
            propAsc = asc;

            var bothImg = '/Design/Images/Datatables/sort_both.png';
            var ascImg = '/Design/Images/Datatables/sort_asc_white.png';
            var descImg = '/Design/Images/Datatables/sort_desc_white.png';

            $('.imgsort').each(function (i, v) {
                $(v).attr('src', bothImg);
            });

            $('.' + imgid).each(function (i, v) {
                if (propAsc) {
                    $(v).attr('src', ascImg);
                } else {
                    $(v).attr('src', descImg);
                }
            });

            filter(false);

            'BaseBlocks-2-Score'
            arrayDataFiltered = arrayDataFiltered.sort(function (a, b) {
                if (prop.indexOf('-') > 0) {
                    var propAux = prop.split('-')[0];
                    var propId = prop.split('-')[1];
                    var subprop = prop.split('-')[2];

                    var valorA = 0;
                    $(a[propAux]).each(function (i, v) {
                        if (v.Id == propId) {
                            valorA = v[subprop];
                            return false;
                        }
                    });

                    var valorB = 0;
                    $(b[propAux]).each(function (i, v) {
                        if (v.Id == propId) {
                            valorB = v[subprop];
                            return false;
                        }
                    });

                    if (asc) return (valorA > valorB) ? 1 : ((valorA < valorB) ? -1 : 0);
                    else return (valorB > valorA) ? 1 : ((valorB < valorA) ? -1 : 0);
                } else {
                    if (asc) return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
                    else return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
                }
            });

            fillRanking(arrayDataFiltered);
        }

        function fillRanking(data) {
            $('#tblRanking tbody').html('');
            $('#tmpltblranking_body').tmpl(data).appendTo('#tblRanking tbody');

            $('.div_popover').popover();
        }

        function showpopover(Id) {
            $('.popover').hide();

            setTimeout(easePieChart, 100);
            IdAux = Id;

        }
    </script>
    <script type="text/x-jquery-tmpl" id="tmpltblranking_head">
        <tr>
            <th style="text-align: center; width: 176px; cursor: pointer;" onclick="sortResults('CityState', undefined, 'sortCity'); return false;">
                <img id="sortCity" class="imgsort sortCity" style='float:left' alt="" title="" src="../Design/Images/Datatables/sort_both.png" />
                <span><asp:Literal ID="Literal10" Text='<%$Resources: Message, cities %>' runat="server" /></span>
            </th>
            {{each BaseBlocks}}
            <th style="text-align: center; width: 215px; cursor: pointer" onclick="sortResults('BaseBlocks-${Id}-Score', undefined, 'sort${Id}'); return false;">${Name}<img id="sort${Id}" class="imgsort sort${Id}" alt="" style='float:left' title="" src="../Design/Images/Datatables/sort_both.png" /></th>
            {{/each}}
            <th style="text-align: center; cursor: pointer;" onclick="sortResults('TotalScore', undefined, 'sortTotal'); return false;"><img id="sortTotal" alt="" title="" class="imgsort sortTotal" style='float:left' src="../Design/Images/Datatables/sort_desc.png" /><asp:Literal ID="Literal11" Text='<%$Resources: Message, total %>' runat="server" /></th>
        </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="tmpltblranking_body">
        <tr>
            <td><a href="/Form/View.aspx?rfid=${IdCrypt}">${CityState}</a></td>
            {{each BaseBlocks}}
            <td align="center">{{each BaseSubblocks}}
                {{if Percent == null}}
                <div class="div_popover" id="div1" onclick="showpopover(${Id}); return false;" style="display: inline-block" rel="popover" data-html="true" data-original-title="${Name}" data-placement="bottom"
                    data-content='<div style="display:none" class="easy-pie-chart styler_infograph easyPieChart pieexemple" data-text="N/A" data-percent="${Percent * 100}" data-trackcolor="#6ed1ff" data-color="#${Color}" data-linewidth="25" data-size="140" data-cap="butt"></div>'>
                    {{else}}
                <div class="div_popover" id="div_popover${Id}" onclick="showpopover(${Id}); return false;" style="display: inline-block" rel="popover" data-html="true" data-original-title="${Name}" data-placement="bottom"
                    data-content='<div style="display:none" class="easy-pie-chart styler_infograph easyPieChart pieexemple" data-text="undefined" data-percent="${Percent * 100}" data-trackcolor="#6ed1ff" data-color="#${Color}" data-linewidth="25" data-size="140" data-cap="butt"></div>'>
                    {{/if}}
                    <div title="${Name}" id="div_block_popover${Id}" class="big-square square" onclick="" style="cursor: pointer; background-color: #${Color};"><span>${Letter}</span></div>
                </div>
                {{/each}}
            </td>
            {{/each}}
            <td style="text-align: right;"><strong>${TotalScore}</strong></td>
        </tr>
    </script>
</asp:Content>
