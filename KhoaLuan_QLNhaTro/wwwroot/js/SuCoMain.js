function toggleDropdown(element) {
    const dropdown = element.querySelector('.dropdown');
    dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
}

document.addEventListener('click', function (event) {
    const optionsIcons = document.querySelectorAll('.options-icon');
    optionsIcons.forEach(icon => {
        if (!icon.contains(event.target)) {
            icon.querySelector('.dropdown').style.display = 'none';
        }
    });
});

document.getElementById('editIncidentModal').addEventListener('hidden.bs.modal', function () {
    console.log('Modal đã được đóng');
});

function openEditModal(url) {
    // Open the modal
    $('#editModal').modal('show');
    // Load content via AJAX
    $('#modalContent').load(url);
}

// Load the edit form in the modal via AJAX
// Open Edit Modal
function openEditModal(id) {
    $.ajax({
        url: `/SuCo/EditSuCo/${id}`,
        type: "GET",
        success: function (html) {
            $("#modalContent").html(html);
            $("#editIncidentModal").modal("show");
        },
        error: function () {
            alert("Không thể tải thông tin sự cố.");
        }
    });
}

// Save Edit
$(document).on("click", "#saveEdit", function () {
    var formData = $("#editIncidentForm").serialize();
    $.ajax({
        url: "/SuCo/EditSuCo",
        type: "POST",
        data: formData,
        success: function (response) {
            if (response.success) {
                $("#editIncidentModal").modal("hide");
                location.reload(); // Reload the list
            } else {
                alert("Cập nhật thất bại.");
            }
        },
        error: function () {
            alert("Có lỗi xảy ra.");
        }
    });
});

// Open Delete Modal
function openDeleteModal(id) {
    if (confirm("Bạn có chắc muốn xóa sự cố này không?")) {
        $.ajax({
            url: "/SuCo/DeleteSuCo",
            type: "POST",
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    location.reload(); // Reload the list
                } else {
                    alert("Xóa thất bại.");
                }
            },
            error: function () {
                alert("Có lỗi xảy ra.");
            }
        });
    }
}


