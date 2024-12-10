
// Toggle dropdown menu for options
function toggleDropdown(element) {
    const dropdown = element.querySelector('.dropdown');
    dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
}

// Close dropdown when clicking outside
document.addEventListener('click', function (event) {
    const optionsIcons = document.querySelectorAll('.options-icon');
    optionsIcons.forEach(icon => {
        if (!icon.contains(event.target)) {
            icon.querySelector('.dropdown').style.display = 'none';
        }
    });
});

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


function updateContractStatus() {
    $(".table-section tbody tr").each(function () {
        /*const createAt = new Date($(this).find("td:nth-child(4)").text()); // Ngày tạo (cột thứ 4)*/
        const createAtText = $(this).find("td:nth-child(4)").text().trim();

        // Tách chuỗi ngày DD/MM/YYYY
        const createAtParts = createAtText.split('/'); // Chia chuỗi thành các phần: [DD, MM, YYYY]

        // Chuyển đổi chuỗi ngày thành đối tượng Date (tháng trong JavaScript bắt đầu từ 0, vì vậy trừ đi 1)
        const createAt = new Date(createAtParts[2], createAtParts[1] - 1, createAtParts[0]);

        const time = parseInt($(this).find("td:nth-child(6)").text()); // Thời hạn hợp đồng (cột thứ 6)
        const endDate = new Date(createAt);
        endDate.setMonth(createAt.getMonth() + time); // Tính ngày kết thúc hợp đồng
        const statusElement = $(this).find("td:nth-child(7) span"); // Cột trạng thái (cột thứ 7)
        console.log("Ngày tạo:", createAt);
        console.log("Ngày kết thúc:", endDate);
        console.log("Ngày hiện tại:", new Date());

        // Nếu ngày hiện tại vượt quá ngày kết thúc hợp đồng, thay đổi trạng thái
        if (new Date() > endDate) {
            statusElement.text("Hết hạn hợp đồng").removeClass("status-active").addClass("status-expired");
        }
    });
}

// Gọi hàm cập nhật trạng thái ngay khi trang load
updateContractStatus();



