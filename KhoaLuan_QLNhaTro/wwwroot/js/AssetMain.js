// Function to open modal
function openModal() {
    document.getElementById('addModal').style.display = 'flex';
}

// Function to close modal
function closeModal() {
    document.getElementById('addModal').style.display = 'none';
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = "none";
}
// Function to handle asset submission
function submitAsset() {
    const assetName = document.getElementById('assetName').value;
    const assetValue = document.getElementById('assetValue').value;
    const quantity = document.getElementById('quantity').value;

    // Perform validation or data submission here
    console.log("Submitted:", assetName, assetValue, quantity);

    closeModal();
}

// Function to open the Edit modal with populated data
function openEditModal(name, value, quantity) {
    document.getElementById('editAssetName').value = name;
    document.getElementById('editAssetValue').value = value;
    document.getElementById('editQuantity').value = quantity;
    document.getElementById('editModal').style.display = 'flex';
}

// Function to open the Delete modal with asset name
function openDeleteModal(name) {
    document.getElementById('deleteMessage').textContent = `Bạn có chắc chắn muốn xóa tài sản "${name}" không?`;
    document.getElementById('deleteModal').style.display = 'flex';
}

// Function to close any modal by id
// function closeModal(modalId) {
//     document.getElementById(modalId).style.display = 'none';
// }

// Function to submit new asset
// function submitAsset() {
//     // Handle adding asset here
//     closeModal('addModal');
// }

// Function to save changes for an edited asset
function saveAssetChanges() {
    // Handle saving asset changes here
    closeModal('editModal');
}

// Function to confirm deletion
function confirmDelete() {
    // Handle asset deletion here
    closeModal('deleteModal');
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