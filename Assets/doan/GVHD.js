$(document).ready(function () {
    //nếu không có thao tác gì thì ẩn đi
    $('#AlertBox').removeClass('hide');

    //Sau khi hiển thị lên thì delay 1s và cuộn lên trên sử dụng slideup
    $('#AlertBox').delay(2000).slideUp(500);

    $('.btnDelete_lstGVHD').click(function () {
        alert("Bạn vui lòng xóa hướng nghiên cứu của giáo viên");
    });

    $('.btnDelete_GVHD').off('click').on('click', function () {
        var doanid = $(this).data('doanid');
        const notice = PNotify.notice({
            title: 'Thông báo',
            text: 'Bạn thật sự muốn xóa?',
            icon: 'fa fa-question-circle',
            width: '360px',
            minHeight: '110px',
            hide: false,
            closer: false,
            sticker: false,
            destroy: true,
            stack: new PNotify.Stack({
                dir1: 'down',
                modal: true,
                firstpos1: 25,
                overlayClose: false
            }),
            modules: new Map([
                ...PNotify.defaultModules,
                [PNotifyConfirm, {
                    confirm: true
                }]
            ])
        });

        notice.on('pnotify:confirm', () =>

            $.ajax({
                data: { ID: $(this).data('id') },
                url: '/DoAn/Delete_GVHD',
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/DoAn/Detail/" + doanid;
                        $('#GVHD').addClass('active').siblings().removeClass('active');
                        PNotify.success({
                            title: 'THÔNG BÁO!!',
                            text: 'Xóa giảng viên tham gia thành công.'
                        });
                    } else {
                        PNotify.error({
                            title: 'THÔNG BÁO!!',
                            text: 'Xóa giảng viên tham gia KHÔNG thành công.'
                        });
                    }
                }
            })

        );


    });

    $('.btn_AddGVHD').click(function (event) {
        var doanid = $(this).data('doanid');
        $.ajax({
            type: 'GET',
            url: "/DoAn/AddGVHD",
            data: {
                doAn_ID: $(this).data('doanid'),
                giaoVien_ID: $("#GiaoVien_ID_GVHD").val()
            },
            dataType: 'Json',
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/DoAn/Detail/" + doanid;
                    $('#GVHD').addClass('active').siblings().removeClass('active');
                    PNotify.success({
                        title: 'THÔNG BÁO!!',
                        text: 'Thêm giảng viên hướng dẫn thành công.'
                    });
                }
            }
        });
    });

    $('.btn_AddHuongNC').click(function (event) {
        var doanid = $(this).data('doanid');
        $.ajax({
            type: 'GET',
            url: "/DoAn/AddHuongNC",
            data: {
                doAn_ID: $(this).data('doanid'),
                giaoVien_ID: $("#GiaoVien_ID_HuongNC").val(),
                huongnc: $('#HuongNghienCuu').val()
            },
            dataType: 'Json',
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/DoAn/Detail/" + doanid;
                    
                    PNotify.success({
                        title: 'THÔNG BÁO!!',
                        text: 'Thêm giảng viên hướng dẫn thành công.'
                    });
                }


                $(window).load(function () {
                    $("#GVHD").tabs("option", "active", 1);
                });
            }
        });
    });

    $(".Khoa_ID_GVHD").change(function () {
        var ID = this.value;
        $.ajax({
            url: '/DoAn/GetChuyenNganhByKhoa/' + ID,
            type: 'get',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                $(".ChuyenNganh_ID_GVHD").empty();
                var def = "<option disabled selected>Chọn chuyên ngành</option>";
                $(".ChuyenNganh_ID_GVHD").append(def);
                $.each(response, function (i, item) {
                    var option = "<option value='" + response[i].ID + "'>" + response[i].ChuyenNganh + "</option>";

                    $(".ChuyenNganh_ID_GVHD").append(option);
                });
                console.log(response)

            }
        });
    });

    $(".ChuyenNganh_ID_GVHD").change(function () {
        var ID = this.value;
        $.ajax({
            url: '/DoAn/GetGVByChuyenNganh/' + ID,
            type: 'get',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                $(".GiaoVien_ID_GVHD").empty();
                var def = "<option disabled selected>Chọn giáo viên</option>";
                $(".GiaoVien_ID_GVHD").append(def);
                $.each(response, function (i, item) {
                    var option = "<option value='" + response[i].ID + "'>" + response[i].TenGiaoVien + "</option>";

                    $(".GiaoVien_ID_GVHD").append(option);
                });

            }
        });
    });
});

