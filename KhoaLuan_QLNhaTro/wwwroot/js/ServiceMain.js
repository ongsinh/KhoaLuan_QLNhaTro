console.log("JavaScript file đã được tải.");

// Mở modal
function openModal() {
    const modal = document.getElementById('addServiceModal');
    if (modal) modal.style.display = 'flex';
}

// Đóng modal
function closeModal() {
    const modal = document.getElementById('addServiceModal');
    if (modal) modal.style.display = 'none';
}

// Đóng modal khi click ra ngoài
window.onclick = function (event) {
    const modal = document.getElementById('addServiceModal');
    if (event.target === modal) {
        closeModal();
    }
};

// Hàm mở modal và tải form chỉnh sửa
function openEditModal(serviceId) {
    console.log("Gửi request với id:", serviceId); // Kiểm tra ID được gửi
    $.ajax({
        url: '/Service/EditService',  // URL action GET
        type: 'GET',
        data: { id: serviceId },
        success: function (response) {
            console.log("Response từ server:", response);
            $('#editServiceFormContainer').html(response);
            $('#editServiceModal').show();
        },
        error: function (xhr, status, error) {
            console.log("Lỗi:", xhr.responseText);  // In chi tiết lỗi từ server
        }
    });

}

$(document).ready(function () {
    $('#editServiceForm').submit(function (e) {
        e.preventDefault();  // Ngăn submit mặc định của form

        var formData = $(this).serialize();  // Lấy dữ liệu form

        $.ajax({
            url: '/Service/EditService',  // Đường dẫn tới action EditService
            type: 'POST',
            data: formData,  // Dữ liệu form gửi đi
            success: function (response) {
                if (response.success) {
                    alert(response.message);  // Hiển thị thông báo thành công
                    window.location.href = '/Service/ServiceMain'; // Điều hướng về trang ServiceMain
                } else {
                    alert("Cập nhật không thành công, vui lòng thử lại!");
                }
            },
            error: function (error) {
                alert("Có lỗi xảy ra khi lưu thay đổi!");
                console.log(error);  // Hiển thị chi tiết lỗi trong console
            }
        });
    });
});

function closeModal(modalId) {
    document.getElementById(modalId).style.display = "none";
}


// Đảm bảo đóng modal khi nhấn ngoài vùng modal
window.onclick = function (event) {
    var modal = document.getElementById('editServiceModal');
    if (event.target == modal) {
        modal.style.display = "none"; // Đóng modal khi nhấn ra ngoài
    }
}

function openDeleteModal(serviceId) {
    // Hiển thị hộp thoại xác nhận
    const isConfirmed = confirm("Bạn có chắc chắn muốn xóa dịch vụ này không?");
    if (isConfirmed) {
        // Nếu người dùng xác nhận, gửi yêu cầu xóa
        fetch(`/Service/DeleteService/${serviceId}`, { method: 'DELETE' })
            .then(response => {
                if (response.ok) {
                    location.reload(); // Làm mới trang sau khi xóa thành công
                } else {
                    alert("Xóa thất bại!");
                }
            })
            .catch(() => alert("Có lỗi xảy ra!"));
    }
}

