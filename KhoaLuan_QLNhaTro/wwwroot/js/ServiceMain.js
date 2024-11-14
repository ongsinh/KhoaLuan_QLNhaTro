function openModal() {
    document.getElementById('addServiceModal').style.display = 'flex';
}

function closeModal() {
    document.getElementById('addServiceModal').style.display = 'none';
}

window.onclick = function (event) {
    const modal = document.getElementById('addServiceModal');
    if (event.target == modal) {
        closeModal();
    }
}

document.querySelector('form').addEventListener('submit', function (event) {
    event.preventDefault();

    const selectedRooms = Array.from(document.querySelectorAll('input[name="applicableRooms"]:checked'))
        .map(room => room.value);

    console.log("Selected Rooms:", selectedRooms);
    // Handle the selectedRooms array as needed, such as sending it to the server
});

function openEditModal() {
    document.getElementById("editServiceModal").style.display = "flex";
}

function openDeleteModal() {
    document.getElementById("deleteConfirmationModal").style.display = "flex";
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = "none";
}

function confirmDelete() {
    // Implement delete logic here
    alert("Service deleted successfully!");
    closeModal('deleteConfirmationModal');
}

