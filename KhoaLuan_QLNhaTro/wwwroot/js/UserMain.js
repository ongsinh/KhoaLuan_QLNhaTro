// Open the modal
function openModal() {
    const addTenantModal = new bootstrap.Modal(document.getElementById('addTenantModal'));
    addTenantModal.show();
}

// Toggle room selection
function selectRoom(element) {
    document.querySelectorAll('.room-item').forEach(room => room.classList.remove('active'));
    element.classList.add('active');
}

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

function openEditModal(event) {
    event.stopPropagation();
    const editModal = new bootstrap.Modal(document.getElementById('editTenantModal'));
    editModal.show();
}

function openDeleteModal(event) {
    event.stopPropagation();
    const deleteModal = new bootstrap.Modal(document.getElementById('deleteTenantModal'));
    deleteModal.show();
}

function saveTenantChanges() {
    alert("Lưu thay đổi cho khách thuê!");
    const editModal = bootstrap.Modal.getInstance(document.getElementById('editTenantModal'));
    editModal.hide();
}

function deleteTenant() {
    alert("Khách thuê đã được xóa!");
    const deleteModal = bootstrap.Modal.getInstance(document.getElementById('deleteTenantModal'));
    deleteModal.hide();
}
