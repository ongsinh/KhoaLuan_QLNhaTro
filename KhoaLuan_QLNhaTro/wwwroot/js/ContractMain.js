$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // Khi nhấn nút "Cộng"
    $("#addContractButton").click(function () {
        const idHouse = $(this).data("id-house"); // Lấy idHouse từ data attribute

        $.ajax({
            url: "/Contract/AddContract",  // GET để lấy form
            type: "GET",
            data: { idHouse: idHouse }, // Gửi idHouse qua AJAX
            success: function (data) {
                $("#modalBody").html(data); // Load nội dung vào modal
                $("#contractModal").modal("show"); // Hiển thị modal
            },
            error: function () {
                alert("Không thể tải form!");
            }
        });
    });
});


