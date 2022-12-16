$(document).ready(function () {
    //nếu không có thao tác gì thì ẩn đi
    $('#AlertBox').removeClass('hide');

    //Sau khi hiển thị lên thì delay 1s và cuộn lên trên sử dụng slideup
    $('#AlertBox').delay(2000).slideUp(500);

    $('.btnDelete_SVTG').off('click').on('click', function () {
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
                url: '/DoAn/Delete_SVTG',
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/DoAn/Detail/" + doanid;
                        PNotify.success({
                            title: 'THÔNG BÁO!!',
                            text: 'Xóa sinh viên tham gia thành công.'
                        });
                    } else {
                        PNotify.error({
                            title: 'THÔNG BÁO!!',
                            text: 'Xóa sinh viên tham gia KHÔNG thành công.'
                        });
                    }
                }
            })

        );


    });

    $('.btn_AddSVTG').click(function (event) {
        var doanid = $(this).data('doanid');
        $.ajax({
            type: 'GET',
            url: "/DoAn/AddSVTG",
            data: {
                doAn_ID: $(this).data('doanid'),
                sinhVien_ID: $("#SinhVien_ID_SVTH").val(),
                namBaoVe_ID: $("#NamBaoVe_ID").val()
            },
            dataType: 'Json',
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/DoAn/Detail/" + doanid;
                    PNotify.success({
                        title: 'THÔNG BÁO!!',
                        text: 'Thêm sinh viên tham gia thành công.'
                    });
                }
            }
        });
    });

    $("#ChuyenNganh_ID_SVTH").change(function () {
        var ID = this.value;
        $.ajax({
            url: '/DoAn/GetLopByChuyenNganh/' + ID,
            type: 'get',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                $("#Lop_ID_SVTH").empty();
                var def = "<option disabled selected>Chọn lớp</option>";
                $("#Lop_ID_SVTH").append(def);
                $.each(response, function (i, item) {
                    var option = "<option value='" + response[i].ID + "'>" + response[i].TenLop + "</option>";

                    $("#Lop_ID_SVTH").append(option);
                });
                console.log(response)
                
            }
        });
    });

    $("#Lop_ID_SVTH").change(function () {
        var ID = this.value;
        $.ajax({
            url: '/DoAn/GetSVByLop/' + ID,
            type: 'get',
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                $("#SinhVien_ID_SVTH").empty();
                var def = "<option disabled selected>Chọn sinh viên</option>";
                $("#SinhVien_ID_SVTH").append(def);
                $.each(response, function (i, item) {
                    var option = "<option value='" + response[i].ID + "'>" + response[i].TenSinhVien + "</option>";

                    $("#SinhVien_ID_SVTH").append(option);
                });

            }
        });
    });
});