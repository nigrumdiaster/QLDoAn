$(document).ready(function () {

    $('.btnEditDiem').click(function (event) {
        $('.editDiem').modal('show');
        //alert($(this).data('id'));
        var ID = $(this).data('id');
        $.ajax({
            url: "/DoAn/GetSinhVienByDoAnID/" + ID,
            data: {},
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                console.log(res.model[0].TenSinhVien);
                $('.TenSV').text(res.model[0].TenSinhVien);
                $('#Edit_Diem').val(res.model[0].Diem);
                $('#ID').val(res.model[0].ID);
            }
        });
    });

    $('.Edit_Diem').click(function (event) {
        var doanid = $(this).data('doanid');
        $.ajax({
            type: 'GET',
            url: "/DoAn/EditDiem",
            data: {
                ID: $('#ID').val(),
                diem: $("#Edit_Diem").val(),
            },
            dataType: 'Json',
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.status == true) {

                    PNotify.success({
                        title: 'THÔNG BÁO!!',
                        text: 'Sửa điểm sinh viên thành công.'
                    });
                    setTimeout(2000);
                    window.location.href = "/DoAn/Detail/" + doanid;
                }
            }
        });
    });

});

