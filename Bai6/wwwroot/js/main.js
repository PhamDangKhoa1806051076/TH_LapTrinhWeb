const apiUrl      = 'http://localhost:5030/api/ProductApi';
const catApiUrl   = 'http://localhost:5030/api/CategoryApi';

let editingProductId = null;
let currentViewId    = null;
let ckEditorInstance = null;
let dataTable        = null;

// ── DOM refs ──────────────────────────────────────────────────
const bookNameInput = document.getElementById('bookName');
const priceInput    = document.getElementById('price');
const categorySelect = document.getElementById('categoryId');
const btnAdd        = document.getElementById('btnAdd');
const btnUpdate     = document.getElementById('btnUpdate');
const btnReset      = document.getElementById('btnReset');
const btnReload     = document.getElementById('btnReload');
const productList   = document.getElementById('productList');
const productCount  = document.getElementById('productCount');
const editModeBadge = document.getElementById('editModeBadge');
const editingIdLabel = document.getElementById('editingIdLabel');

// ── Init ──────────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    // Init CKEditor
    ClassicEditor
        .create(document.getElementById('description'), {
            toolbar: ['heading', '|', 'bold', 'italic', 'underline', 'strikethrough',
                      '|', 'bulletedList', 'numberedList', '|',
                      'blockQuote', 'link', '|', 'undo', 'redo'],
            placeholder: 'Nhập mô tả sản phẩm...'
        })
        .then(editor => { ckEditorInstance = editor; })
        .catch(err => console.error('CKEditor error:', err));

    // Init DataTable
    dataTable = $('#tblProducts').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json'
        },
        responsive: true,
        pageLength: 10,
        order: [[0, 'desc']],
        columnDefs: [
            { orderable: false, targets: 5 }
        ]
    });

    loadCategories();
    fetchProducts();

    btnAdd.addEventListener('click', e => { e.preventDefault(); addProduct(); });
    btnUpdate.addEventListener('click', e => { e.preventDefault(); updateProduct(); });
    btnReset.addEventListener('click', e => { e.preventDefault(); resetForm(); });
    btnReload.addEventListener('click', () => fetchProducts());

    document.getElementById('btnEditFromModal').addEventListener('click', () => {
        bootstrap.Modal.getInstance(document.getElementById('modalViewDetailInfo'))?.hide();
        if (currentViewId) editProduct(currentViewId);
    });
});

// ── Load Categories vào dropdown ──────────────────────────────
function loadCategories() {
    fetch(catApiUrl)
        .then(r => r.json())
        .then(cats => {
            categorySelect.innerHTML = '<option value="">-- Chọn danh mục --</option>';
            cats.forEach(c => {
                const opt = document.createElement('option');
                opt.value = c.id;
                opt.textContent = c.name;
                categorySelect.appendChild(opt);
            });
        })
        .catch(err => console.error('Lỗi load categories:', err));
}

// ── Helpers ───────────────────────────────────────────────────
function handleResponse(response) {
    if (!response.ok) throw new Error('Network error ' + response.status);
    if (response.status === 204) return {};
    return response.json();
}

function showToast(msg, type = 'success') {
    const toastEl  = document.getElementById('appToast');
    const toastMsg = document.getElementById('toastMsg');
    toastEl.className = `toast align-items-center text-white border-0 bg-${type}`;
    toastMsg.textContent = msg;
    new bootstrap.Toast(toastEl, { delay: 2000 }).show();
}

function fmtPrice(val) {
    return parseFloat(val).toLocaleString('vi-VN', { minimumFractionDigits: 0 }) + ' ₫';
}

function escHtml(str) {
    if (!str) return '';
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

// ── Fetch & Display ───────────────────────────────────────────
function fetchProducts() {
    fetch(apiUrl)
        .then(handleResponse)
        .then(data => displayProducts(data))
        .catch(err => {
            console.error(err);
            Swal.fire({
                icon: 'error', title: 'Lỗi kết nối',
                text: 'Không thể kết nối đến server. Hãy đảm bảo backend đang chạy!'
            });
        });
}

function displayProducts(products) {
    productCount.textContent = products.length;

    // Xóa dữ liệu cũ trong DataTable
    dataTable.clear();

    products.forEach(p => {
        const shortDesc = p.description
            ? p.description.replace(/<[^>]*>/g, '').substring(0, 60) + (p.description.length > 60 ? '...' : '')
            : '—';

        const actions = `
            <button class="btn btn-danger btn-sm me-1 delete-btn" data-id="${p.id}" title="Xóa sản phẩm">
                <i class="fa-solid fa-trash"></i> Xoá
            </button>
            <button class="btn btn-warning btn-sm me-1 text-white edit-btn" data-id="${p.id}" title="Sửa sản phẩm">
                <i class="fa-solid fa-pen-to-square"></i> Sửa
            </button>
            <button class="btn btn-primary btn-sm view-btn" data-id="${p.id}" title="Xem chi tiết">
                <i class="fa-solid fa-eye"></i> Xem
            </button>`;

        dataTable.row.add([
            `<span class="badge bg-secondary">${p.id}</span>`,
            `<strong>${escHtml(p.name)}</strong>`,
            p.categoryName
                ? `<span class="badge bg-info text-dark">${escHtml(p.categoryName)}</span>`
                : `<span class="text-muted">—</span>`,
            `<span class="fw-bold text-primary">${fmtPrice(p.price)}</span>`,
            `<span class="text-muted" title="${escHtml(p.description ? p.description.replace(/<[^>]*>/g,'') : '')}">${escHtml(shortDesc)}</span>`,
            `<div class="d-flex gap-1 justify-content-center">${actions}</div>`
        ]);
    });

    dataTable.draw();
    attachRowEvents();
}

function attachRowEvents() {
    // Dùng event delegation vì DataTables tái tạo DOM khi phân trang
    $('#tblProducts tbody').off('click').on('click', '.delete-btn', function() {
        confirmDelete($(this).data('id'));
    }).on('click', '.edit-btn', function() {
        editProduct($(this).data('id'));
    }).on('click', '.view-btn', function() {
        viewProduct($(this).data('id'));
    });
}

// ── Validation ────────────────────────────────────────────────
function validate() {
    const name     = bookNameInput.value.trim();
    const priceVal = priceInput.value.trim();
    const desc     = ckEditorInstance ? ckEditorInstance.getData() : '';
    const catId    = categorySelect.value;

    if (!name) {
        Swal.fire({ icon: 'warning', title: 'Thiếu thông tin', text: 'Vui lòng nhập tên sản phẩm!' });
        bookNameInput.focus(); return null;
    }
    const price = parseFloat(priceVal);
    if (!priceVal || isNaN(price) || price < 0) {
        Swal.fire({ icon: 'warning', title: 'Giá không hợp lệ', text: 'Vui lòng nhập giá sản phẩm hợp lệ (đơn vị đồng)!' });
        priceInput.focus(); return null;
    }
    if (!desc || desc.replace(/<[^>]*>/g, '').trim() === '') {
        Swal.fire({ icon: 'warning', title: 'Thiếu thông tin', text: 'Vui lòng nhập mô tả sản phẩm!' });
        return null;
    }

    return {
        name,
        price,
        description: desc,
        categoryId: catId ? parseInt(catId) : null
    };
}

// ── CRUD ──────────────────────────────────────────────────────
function addProduct() {
    const data = validate();
    if (!data) return;

    fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
    .then(handleResponse)
    .then(() => {
        Swal.fire({ icon: 'success', title: 'Thành công', text: `Đã thêm "${data.name}"!`, timer: 1800, showConfirmButton: false });
        resetForm();
        fetchProducts();
    })
    .catch(() => Swal.fire({ icon: 'error', title: 'Lỗi', text: 'Không thể thêm sản phẩm!' }));
}

function editProduct(id) {
    fetch(`${apiUrl}/${id}`)
        .then(handleResponse)
        .then(p => {
            editingProductId = p.id;
            currentViewId    = p.id;
            bookNameInput.value = p.name;
            priceInput.value    = p.price;
            categorySelect.value = p.categoryId ?? '';
            if (ckEditorInstance) ckEditorInstance.setData(p.description || '');

            btnAdd.style.display    = 'none';
            btnUpdate.style.display = 'inline-block';
            editModeBadge.style.display = 'inline-block';
            editingIdLabel.textContent  = p.id;

            document.getElementById('formSection').scrollIntoView({ behavior: 'smooth' });
            bookNameInput.focus();
        })
        .catch(() => Swal.fire({ icon: 'error', title: 'Lỗi', text: 'Không thể lấy thông tin sản phẩm!' }));
}

function updateProduct() {
    if (!editingProductId) return;
    const data = validate();
    if (!data) return;
    data.id = editingProductId;

    fetch(`${apiUrl}/${editingProductId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
    .then(handleResponse)
    .then(() => {
        Swal.fire({ icon: 'success', title: 'Cập nhật thành công', text: `"${data.name}" đã được cập nhật!`, timer: 1800, showConfirmButton: false });
        resetForm();
        fetchProducts();
    })
    .catch(() => Swal.fire({ icon: 'error', title: 'Lỗi', text: 'Không thể cập nhật sản phẩm!' }));
}

function confirmDelete(id) {
    Swal.fire({
        title: 'Xác nhận xóa?',
        html: `Bạn có chắc muốn xóa sản phẩm <strong>ID: ${id}</strong>?<br><small class="text-muted">Hành động này không thể hoàn tác.</small>`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: '<i class="fa-solid fa-trash me-1"></i>Đồng ý, xóa',
        cancelButtonText: 'Hủy bỏ'
    }).then(result => { if (result.isConfirmed) deleteProduct(id); });
}

function deleteProduct(id) {
    fetch(`${apiUrl}/${id}`, { method: 'DELETE' })
        .then(handleResponse)
        .then(() => {
            showToast('Xóa sản phẩm thành công!', 'success');
            if (editingProductId === parseInt(id)) resetForm();
            fetchProducts();
        })
        .catch(() => Swal.fire({ icon: 'error', title: 'Lỗi', text: 'Không thể xóa sản phẩm!' }));
}

function viewProduct(id) {
    currentViewId = parseInt(id);
    fetch(`${apiUrl}/${id}`)
        .then(handleResponse)
        .then(p => {
            document.getElementById('detailId').textContent     = p.id;
            document.getElementById('detailName').textContent   = p.name;
            document.getElementById('detailNameTd').textContent = p.name;
            document.getElementById('detailPrice').textContent  = fmtPrice(p.price);
            document.getElementById('detailCategory').textContent    = p.categoryName || '—';
            document.getElementById('detailCategoryBadge').textContent = p.categoryName || '—';
            // Render HTML từ CKEditor
            document.getElementById('detailDescription').innerHTML = p.description || '—';

            new bootstrap.Modal(document.getElementById('modalViewDetailInfo')).show();
        })
        .catch(() => Swal.fire({ icon: 'error', title: 'Lỗi', text: 'Không thể tải chi tiết sản phẩm!' }));
}

function resetForm() {
    bookNameInput.value     = '';
    priceInput.value        = '';
    categorySelect.value    = '';
    if (ckEditorInstance) ckEditorInstance.setData('');
    editingProductId        = null;

    btnAdd.style.display    = 'inline-block';
    btnUpdate.style.display = 'none';
    editModeBadge.style.display = 'none';
    editingIdLabel.textContent  = '?';
}
