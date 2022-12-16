$(document).ready(function () {

    $('.btnDelete_lstGVPB').off('click').on('click', function () {
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
                url: '/DoAn/Delete_GVPB',
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {

                        PNotify.success({
                            title: 'THÔNG BÁO!!',
                            text: 'Xóa giảng viên trong hội đồng thành công.'
                        });
                        setTimeout(2000);
                        window.location.href = "/DoAn/Detail/" + doanid;
                    } else {
                        PNotify.error({
                            title: 'THÔNG BÁO!!',
                            text: 'Xóa giảng viên trong hội đồng KHÔNG thành công.'
                        });
                    }
                }
            })

        );


    });

    $('.btn_AddGVPB').click(function (event) {
        var doanid = $(this).data('doanid');
        $.ajax({
            type: 'GET',
            url: "/DoAn/AddGVPB",
            data: {
                doAn_ID: $(this).data('doanid'),
                giaoVien_ID: $("#GiaoVien_ID_GVPB").val(),
                nhanxet: $("#NhanXet_GVPB").val(),
            },
            dataType: 'Json',
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.status == true) {

                    PNotify.success({
                        title: 'THÔNG BÁO!!',
                        text: 'Thêm giảng viên phản biện thành công.'
                    });
                    setTimeout(2000);
                    window.location.href = "/DoAn/Detail/" + doanid;
                }
            }
        });
    });

});

