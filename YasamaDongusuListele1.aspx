<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CiroHesaplama.Models.DIO.MerkezListeModel>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Merkez Listeleme İşlemi</title>

    <link rel="apple-touch-icon" sizes="76x76" href="../../Content/assets/img/apple-icon.png">
    <link rel="icon" type="image/png" sizes="96x96" href="../../Content/assets/img/favicon.png">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
    <meta name="viewport" content="width=device-width" />
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/assets/css/animate.min.css" rel="stylesheet" />
    <link href="../../Content/assets/css/paper-dashboard.css" rel="stylesheet" />
    <link href="../../Content/assets/css/demo.css" rel="stylesheet" />

    <link href="http://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" rel="stylesheet">
    <link href='https://fonts.googleapis.com/css?family=Muli:400,300' rel='stylesheet' type='text/css'>
    <link href="../../Content/assets/css/themify-icons.css" rel="stylesheet" />



    <link href="../../Content/datatblesayfalama/bootstrap.min.css" rel="stylesheet" />


    <link href="../../Content/datatblesayfalama/dataTables.bootstrap.min.css" rel="stylesheet" />
     <link href="../../Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
    <link href="../../Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="../../Content/datatblesayfalama/dataTables.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <%--  <script src="../../Content/datatblesayfalama/jquery-1.12.3.js"></script>--%>
    <script src="../../Content/datatblesayfalama/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#xx').DataTable({
                // "scrollY": "500px",
                //"processing": true,
                //"scrollCollapse": true,
                "paging": true

            });
        });

    </script>





    <style type="text/css">
        .gizle {
            display: none;
        }

        @font-face {
            font-family: 'AbakuTLSymSansRegular';
            src: url('fonts/abakutlsymsans-regular-AbakuTLSymSans.eot');
            src: url('fonts/abakutlsymsans-regular-AbakuTLSymSans.eot?#iefix') format('embedded-opentype'), url('fonts/abakutlsymsans-regular-AbakuTLSymSans.ttf') format('truetype'), url('fonts/abakutlsymsans-regular-AbakuTLSymSans.svg#AbakuTLSymSansRegular') format('svg');
            font-weight: normal;
            font-style: normal;
        }

        .TL:after {
            font-family: "AbakuTLSymSansRegular",Verdana,Arial;
            content: "tl";
            padding-left: 3px;
            color: #ff6a00;
        }
    </style>
</head>
<body>
    <div class="container">
        <%using (Html.BeginForm("YasamaDongusuListele1", "Merkez", FormMethod.Post, new { @name = "formYasamaDongusuListele1" }))
          {%>

        <div class="row">
            <div class="col-md-12">

                <div class="panel-warning">
                    <div class="panel-heading" style="background-color: #ff6a00; text-align: center; color: #000000">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-2"></div>
                                <div class="col-md-9 text-center" style="font-family: 'Segoe Script'; font-weight: bolder; color: #ffffff">
                                    <h4>Merkez</h4>
                                </div>

                                <div class="col-md-1 text-center">
                                    <%:Html.ActionLink("Çıkış Yap", "LogOut", "Login", new { @class = "btn btn-primary" })%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body" style="color: #000000">
                        <a class="btn btn-warning btn-md" style="color: #000000" href="AnaSayfa">Anasayfaya Git</a>

                        <div class="panel panel-warning" style="color: #000000">
                            <div class="panel-body">

                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <table class="table table-striped table-bordered">
                                            <tr>
                                                <td>Şube Seçiniz</td>
                                                <td>
                                                    <%:Html.DropDownList("rollerim",null,"---seçiniz----", new { @class = "form-control"}) %>
                                                   
                                                </td>
                                            </tr>

                                           <tr>
                                                <td>
                                                    <label class="control-label ">Başlangıç Tarihi</label></td>
                                                <td>
                                                    <%: Html.TextBoxFor(model => model.Filtreleme.DonguTarih1Filtre, new { Value ="tarih seçiniz", @class = "datefield",   @readonly = "readonly",Required="" })%></td>
                                            </tr>
                                            <tr>
                                                <td><label class="control-label ">Bitiş Tarihi</label></td>
                                                <td><%: Html.TextBoxFor(model => model.Filtreleme.DonguTarih2Filtre, new { Value ="tarih seçiniz", @class = "datefield",   @readonly = "readonly",Required="" })%></td>
                                           
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <input class="btn btn-danger btn-sm" type="submit" value="İndir" /></td>

                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-4 gizle">
                                        <table class="table table-striped table-bordered gizle">
                                            <tr>
                                                <td>Toplam Çek</td>
                                                <td class="TL"><%:(string)ViewBag.tc %></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Nakit</td>
                                                <td class="TL"><%:(string)ViewBag.tn %></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Havale</td>
                                                <td class="TL"><%:(string)ViewBag.th%></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Kredi Kartı</td>
                                                <td class="TL"><%:(string)ViewBag.tkk%></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Günlük Tahsil Tutar</td>
                                                <td class="TL"><%:(string)ViewBag.tgt %></td>
                                            </tr>

                                        </table>
                                    </div>
                                    <div class="col-md-4 gizle">
                                        <table class="table table-striped table-bordered">
                                            <tr>
                                                <td>Toplam Fatura Sayısı</td>
                                                <td><%:(string)ViewBag.tfs %></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Fatura Tutarı</td>
                                                <td class="TL"><%:(string)ViewBag.tft  %></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Harcama Adet</td>
                                                <td><%:(string)ViewBag.tha %></td>
                                            </tr>
                                            <tr>
                                                <td>Toplam Harcama Tutar</td>
                                                <td class="TL"><%:(string)ViewBag.tht %></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table id="xx" class="table table-striped table-bordered gizle">
                            <thead>
                                <tr>

                                    <th>Şube Adı</th>
                                    <th style="width: 120px;">Tarih</th>
                                    <th>Çek</th>
                                    <th>Nakit</th>
                                    <th>Havale</th>
                                    <th>Kredi Kartı</th>
                                    <th>Günlük Tahsil</th>
                                    <th>Fatura Sayısı</th>
                                    <th>Fatura Tutar</th>
                                    <th>Harcama Adet</th>
                                    <th>Harcama Tutar</th>
                                </tr>
                            </thead>
                            <% foreach (var item in Model.MerkezListeModelimiz)
                               { %>
                            <tr>
                                <td class="gizle"><%=Html.DisplayName(item.CiroNo.ToString()) %></td>
                                <td><%=Html.DisplayName(item.SubeAdi) %></td>
                                <td style="width: 120px;"><%=Html.DisplayFor(c=>item.Tarih)%></td>
                                <td class="TL"><%=Html.DisplayFor(c => item.Cek)%></td>
                                <td class="TL"><%=Html.DisplayFor(c=>item.Nakit) %></td>
                                <td class="TL"><%=Html.DisplayFor(c=>item.Havale) %></td>
                                <td class="TL"><%=Html.DisplayFor(c=>item.KrediKarti) %></td>
                                <td class="TL"><%=Html.DisplayFor(c=>item.GunlukTahsilTutar) %></td>
                                <td><%=Html.DisplayName(item.FaturaSayisi.ToString()) %></td>

                                <td class="TL"><%= Html.DisplayFor(c => item.FaturaTutari) %></td>

                                <td><%=Html.DisplayName(item.HarcamaAdet.ToString()) %></td>
                                <td class="TL"><%=Html.DisplayFor(c =>item.HarcamaTutar) %></td>

                            </tr>
                            <% } %>
                        </table>
                    </div>
                </div>

            </div>
        </div>





        <%} %>
        <script src="//code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
        <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".datefield").datepicker({ dateFormat: 'dd.mm.yy', changeYear: true });
            });
        </script>
    </div>
</body>
</html>
