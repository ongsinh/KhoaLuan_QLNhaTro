document.getElementById("addHouseButton").addEventListener("click", function () {
    // Mở modal khi nút thêm được nhấn
    $('#addHouseModal').modal('show');
});

// Hàm tạo các ô nhập phòng khi thay đổi số tầng
function generateFloorInputs() {
    const floorNumber = parseInt(document.getElementById("floorNumber").value);
    const floorInputsContainer = document.getElementById("floorInputs");

    floorInputsContainer.innerHTML = ""; // Clear previous inputs

    if (floorNumber > 0) {
        for (let i = 1; i <= floorNumber; i++) {
            const floorDiv = document.createElement("div");
            floorDiv.className = "form-group";
            floorDiv.innerHTML = `
                                <label for="floor_${i}">Số phòng của tầng ${i}</label>
                                <input type="number" class="form-control" id="floor_${i}" name="RoomsPerFloor[${i - 1}]" placeholder="Nhập số phòng tầng ${i}" required />
                            `;
            floorInputsContainer.appendChild(floorDiv);
        }
    }
}

$(document).ready(function () {
    $('#addHouseForm').submit(function (event) {
        event.preventDefault(); // Ngừng form submit mặc định

        var formData = new FormData(this);  // Lấy tất cả dữ liệu trong form

        // Debug thông tin FormData
        console.log("FormData:", formData);

        $.ajax({
            url: '/House/CreateHouseAndRooms',  // Đường dẫn action
            method: 'POST',
            data: formData,  // Dữ liệu gửi đi từ formData
            processData: false,  // Đảm bảo không xử lý dữ liệu tự động
            contentType: false,  // Không dùng content-type mặc định
            success: function (response) {
                if (response.success) {
                    // Hiển thị tên nhà trọ mới tại sidebar
                    $('.property-name').text(response.houseName);

                    // Lưu houseId vào session để dùng cho các yêu cầu tiếp theo
                    sessionStorage.setItem('houseId', response.houseId);
                    sessionStorage.setItem('houseName', response.houseName);

                    window.location.href = `/Room/RoomMain?houseId=${response.houseId}`;
                } else {
                    alert('Lỗi khi tạo nhà trọ.');
                }
            },
            error: function () {
                alert('Có lỗi xảy ra khi gửi dữ liệu.');
            }
        });
    });
});
// Khi modal đóng
// Đảm bảo modal đóng được khi nhấn "x" hoặc nút "Đóng"
$(document).on('click', '[data-dismiss="modal"]', function () {
    $(this).closest('.modal').modal('hide');
});



//function closeModal(modalId) {
//    $('#' + modalId).hide();
//}
$(document).ready(function () {
    // Khi click vào div chứa thông tin nhà trọ
    $('#iconText, #houseIcon, .property-name').click(function () {
        loadHouseList();
    });

    function loadHouseList() {
        // Gửi AJAX request để lấy danh sách nhà trọ
        $.ajax({
            url: '/House/HouseList',  // Đường dẫn đến Action HouseList
            method: 'GET',
            success: function (houses) {
                // Nếu có nhà trọ
                if (houses.length > 0) {
                    let houseListHtml = '';
                    houses.forEach(function (house) {
                        houseListHtml += `
                <div class="house-item" data-house-id="${house.id}"> <!-- Dùng house.id từ JSON -->
                    <div class="form-row">
                        <div class="form-group col-md-8">
                            <strong>${house.name}</strong>
                            <p>${house.address}</p>
                        </div>
                        <div class="form-group col-md-4">
                            <button class="btn btn-delete" data-house-id="${house.id}"><i class="fas fa-trash-alt"></i></button>
                            
                            <a href="#" class="btn-select" data-house-id="${house.id}">
                                <i class="fas fa-arrow-right"></i>
                            </a>
                        </div>
                    </div>
                </div>`;
                    });

                    // Hiển thị danh sách nhà trọ vào modal
                    $('#houseListModal .modal-body').html(houseListHtml);
                } else {
                    $('#houseListModal .modal-body').html('<p>Không có nhà trọ nào.</p>');
                }
                // Mở modal
                $('#houseListModal').modal('show');
            },
            error: function () {
                alert('Lỗi khi lấy dữ liệu nhà trọ');
            }
        });
    }
    // Khi bấm vào nút "Chọn nhà trọ"
    $(document).on('click', '.btn-select', function (event) {
        event.preventDefault();
        var houseId = $(this).data('house-id');  // Lấy houseId từ data-house-id

        console.log("houseId: ", houseId); // Kiểm tra giá trị houseId

        // Gửi yêu cầu AJAX để lưu thông tin nhà trọ vào session
        $.ajax({
            url: '/House/SetHouseSession',  // Đường dẫn tới action SetHouseSession
            method: 'POST',
            data: { houseId: houseId },
            success: function (response) {
                if (response.success) {
                    // Chuyển hướng tới trang RoomMain với houseId
                    window.location.href = `/Room/RoomMain?houseId=${houseId}`;
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Có lỗi khi lưu thông tin nhà trọ');
            }
        });
    });

    //Xóa nhà trọ
    $(document).off('click', '.btn-delete').on('click', '.btn-delete', function () {
        const houseId = $(this).data('house-id'); // Lấy houseId từ nút xóa

        if (confirm('Bạn có chắc chắn muốn xóa nhà trọ này?')) {
            $.ajax({
                url: '/House/DeleteHouse',
                method: 'POST',
                data: { houseId: houseId },
                success: function (response) {
                    if (response.success) {
                        alert('Xóa nhà trọ thành công!');
                        loadHouseList(); // Tải lại danh sách nhà trọ
                    } else {
                        alert('Lỗi khi xóa nhà trọ: ' + response.message);
                    }
                },
                error: function () {
                    alert('Có lỗi xảy ra khi xóa nhà trọ.');
                }
            });
        }
    });
});

//function showEditModalHouse(houseId) {
//    // Mở modal chỉnh sửa và điền thông tin nhà trọ
//    $.ajax({
//        url: '/House/GetHouseById',  // Lấy thông tin nhà trọ qua AJAX
//        method: 'GET',
//        data: { id: houseId },
//        success: function (response) {
//            if (response.success) {
//                // Điền thông tin vào modal chỉnh sửa
//                $('#editHouseName').val(response.house.Name);
//                $('#editHouseAddress').val(response.house.Address);
//                $('#editHouseId').val(response.house.Id);
//                $('#editHouseModal').modal('show');
//            }
//        }
//    });
//}



