var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#TenDoAn").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Search/SearchDoAn",//Link lấy dữ liệu gợi ý
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                        //response($.map(data, function (item) {
                        //    return {
                        //        value: item.Product_Name,
                        //        label: item.Image
                        //    }
                        //}));
                    }
                });
            },
            focus: function (event, ui) {
                $("#TenDoAn").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#TenDoAn").val(ui.item.label);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    //.append("<div>" + item.value + "<br>" + item.label + "</div>")
                    .append("<div>" + item.value + "</div>")
                    .appendTo(ul);
            };

        $("#GVHD").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Search/SearchGVHD",//Link lấy dữ liệu gợi ý
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                        //response($.map(data, function (item) {
                        //    return {
                        //        value: item.Product_Name,
                        //        label: item.Image
                        //    }
                        //}));
                    }
                });
            },
            focus: function (event, ui) {
                $("#GVHD").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#GVHD").val(ui.item.label);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    //.append("<div>" + item.value + "<br>" + item.label + "</div>")
                    .append("<div>" + item.value + "</div>")
                    .appendTo(ul);
            };

        $("#TenGV").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Search/SearchGVHD",//Link lấy dữ liệu gợi ý
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                        //response($.map(data, function (item) {
                        //    return {
                        //        value: item.Product_Name,
                        //        label: item.Image
                        //    }
                        //}));
                    }
                });
            },
            focus: function (event, ui) {
                $("#TenGV").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#TenGV").val(ui.item.label);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    //.append("<div>" + item.value + "<br>" + item.label + "</div>")
                    .append("<div>" + item.value + "</div>")
                    .appendTo(ul);
            };

        $("#TenSinhVien").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Search/SearchSinhVien",//Link lấy dữ liệu gợi ý
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                        //response($.map(data, function (item) {
                        //    return {
                        //        value: item.Product_Name,
                        //        label: item.Image
                        //    }
                        //}));
                    }
                });
            },
            focus: function (event, ui) {
                $("#TenSinhVien").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#TenSinhVien").val(ui.item.label);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    //.append("<div>" + item.value + "<br>" + item.label + "</div>")
                    .append("<div>" + item.value + "</div>")
                    .appendTo(ul);
            };
    }
}
common.init();

$(document).ready(function () {

    $('#btn_SearchDoAn').click(function (event) {
        $.ajax({
            url: "/Search/Result_DoAnGVHD",
            data: {
                tendoan: $('#TenDoAn').val(),
                giaovien: $('#GVHD').val()
            },
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                console.log(response);
                $("#table_DoAn_GVHD").empty();
                if (response.length != 0) {
                    var dem = 1;
                    $.each(response, function (i, item) {
                        var table = '<tr>' +
                            '<td>' + dem + '</td>' +
                            '<td>' + response[i].MaDoAn + '</td>' +
                            '<td>' + response[i].TenDoAn + '</td>' +
                            '<td>' + response[i].GVHD + '</td>' +
                            '<td>' + response[i].ChuyenNganh + '</td>' +
                            '</tr>';
                        dem++;
                        $("#table_DoAn_GVHD").append(table);
                    });
                } else {
                    var table = '<tr class="odd"><td valign="top" colspan="5" class="dataTables_empty">Không có dữ liệu bạn tìm kiếm</td></tr>';
                    $("#table_DoAn_GVHD").append(table);
                }

            }
        });
    });
    //<tr class="odd"><td valign="top" colspan="5" class="dataTables_empty">No data available in table</td></tr>
    $('#btn_SearchTenGV').click(function (event) {
        $.ajax({
            url: "/Search/Result_GVHD",
            data: {
                giaovien: $('#TenGV').val()
            },
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                console.log(response);
                $("#table_GVHD").empty();
                if (response.length != 0) {
                    var dem = 1;
                    $.each(response, function (i, item) {
                        var table = '<tr>' +
                            '<td>' + dem + '</td>' +
                            '<td>' + response[i].TenGiaoVien + '</td>' +
                            '<td>' + response[i].TenDoAn + '</td>' +
                            '<td>' + response[i].ChuyenNganh + '</td>' +
                            '</tr>';
                        dem++;
                        $("#table_GVHD").append(table);
                    });
                } else {
                    var table = '<tr class="odd"><td valign="top" colspan="4" class="dataTables_empty">Không có dữ liệu bạn tìm kiếm</td></tr>';
                    $("#table_GVHD").append(table);
                }

            }
        });
    });

    $('#btn_SearchSinhVien').click(function (event) {
        $.ajax({
            url: "/Search/Result_SinhVien",
            data: {
                tensinhvien: $('#TenSinhVien').val()
            },
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                $("#table_SinhVien").empty();
                console.log(response);
                if (response.length != 0) {
                    var dem = 1;
                    $.each(response, function (i, item) {
                        var table = '<tr>' +
                            '<td>' + dem + '</td>' +
                            '<td>' + response[i].TenSinhVien + '</td>' +
                            '<td>' + response[i].TenDoAn + '</td>' +
                            '<td>' + response[i].ChuyenNganh + '</td>' +
                            '<td> Đợt ' + response[i].DotBaoVe + ' năm ' + response[i].NamBaoVe + '</td>' +
                            '</tr>';
                        dem++;
                        $("#table_SinhVien").append(table);
                    });
                } else {
                    var table = '<tr class="odd"><td valign="top" colspan="5" class="dataTables_empty">Không có dữ liệu bạn tìm kiếm</td></tr>';
                    $("#table_SinhVien").append(table);
                }

            }
        });
    });
});