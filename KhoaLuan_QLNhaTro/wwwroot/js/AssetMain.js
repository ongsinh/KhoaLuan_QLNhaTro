
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

function openModal(idHouse) {
    if (idHouse) {
        // Mở modal và xử lý với idHouse
        console.log('ID Nhà Trọ:', idHouse);
        // Code mở modal ở đây...
    } else {
        console.error('Không có idHouse');
    }
    const modal = $('#addModal');
    if (!modal.length) {
        alert("Không tìm thấy modal.");
        return;
    }

    $.ajax({
        url: '/Asset/AddAsset',
        type: 'GET',
        data: { idHouse: idHouse },
        success: function (data) {
            $('#addAssetFormContainer').html(data); // Load nội dung form vào modal
            modal.show(); // Hiển thị modal
        },
        error: function () {
            alert("Không thể tải form thêm tài sản.");
        }
    });
}

// Hàm đóng modal
function closeModal(modalId) {
    $("#" + modalId).hide();
}



function closeModal(modalId) {
    document.getElementById(modalId).style.display = "none";
}
/// Hiển thị modal và tải form khi nhấn "Sửa"
function openEditModal(assetId) {
    console.log("Asset ID:", assetId); // Kiểm tra giá trị assetId
    $.ajax({
        url: '/Asset/EditAsset/'/* + assetId*/,  // Đường dẫn tới action trả về form
        type: 'GET',
        data: { id: assetId },
        success: function (response) {
            console.log(response);  // Kiểm tra phản hồi từ server
            if (response.success === false) {
                alert(response.message);
            } else {
                $('#editAssetFormContainer').html(response);
                $('#editModal').show();
                //location.reload();
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi khi tải thông tin tài sản.");
        }
    });
}

// Gửi yêu cầu cập nhật tài sản qua AJAX

$(document).ready(function () {
    $('#editAssetForm').submit(function (e) {
        e.preventDefault();  // Ngăn submit mặc định của form

        var formData = $(this).serialize();  // Lấy dữ liệu form

        $.ajax({
            url: '/Asset/EditAsset',  // Đường dẫn tới action EditService
            type: 'POST',
            data: formData,  // Dữ liệu form gửi đi
            success: function (response) {
                if (response.success) {
                    alert(response.message);  // Hiển thị thông báo thành công
                    window.location.href = '/Asset/AssetMain'; // Điều hướng về trang ServiceMain
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

function openDeleteModal(AssetId) {
    // Hiển thị hộp thoại xác nhận
    const isConfirmed = confirm("Bạn có chắc chắn muốn xóa dịch vụ này không?");
    if (isConfirmed) {
        // Nếu người dùng xác nhận, gửi yêu cầu xóa
        fetch(`/Asset/DeleteAsset/${AssetId}`, { method: 'DELETE' })
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

//Nhà trọ
$('#addHouseForm').submit(function (e) {
    e.preventDefault();

    const formData = $(this).serialize();

    $.ajax({
        url: $(this).attr('action'),
        method: $(this).attr('method'),
        data: formData,
        success: function (response) {
            if (response.success) {
                alert('Thêm nhà trọ thành công!');
                window.location.href = response.redirectUrl; // Chuyển hướng đến giao diện RoomMain
            } else {
                alert('Thêm nhà trọ thất bại. Vui lòng kiểm tra lại.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra. Vui lòng thử lại sau.');
        }
    });
});

