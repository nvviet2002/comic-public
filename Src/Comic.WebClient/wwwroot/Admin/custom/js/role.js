
$(document).ready(async function () {
    await listRoleAsync();
    await listPermissionAsync();
    $("#addRoleForm").on("submit", async function (e) {
        e.preventDefault();
        await addRoleAsync();
    });
    $("#addRoleModal").on("show.bs.modal", async function (e) {
        await resetAddRoleForm();
    });
    ///update
    $("#editRoleForm").on("submit", async function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        await updateRoleAsync(id);
    });
    $("#editRoleModal").on("show.bs.modal", async function (e) {
        await resetEditRoleForm();
    });
    
});

//list role
async function listRoleAsync() {
    $.ajax({
        url: `${ApiServer}/api/role/list`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            await showRoleListAsync(data.data);
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await listRoleAsync();
            }
        }
    });
}

async function showRoleListAsync(roles) {
    var html = "";
    roles.forEach((role) => {
        html += generateRoleHtml(role);
        console.log(role);
    });
    html += generateAddRoleHtml();
    $("#roleContainer").html(html);
    await setDeleteRoleBtnClick();
    await setEditRoleBtnClick();
}
function generateRoleHtml(role) {
    const html = `<div class="col-xl-4 col-lg-6 col-md-6">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex justify-content-between">
                        <h6 class="fw-normal mb-2">Tổng ${role.userCount} người dùng</h6>
                        <ul class="list-unstyled d-flex align-items-center avatar-group mb-0">
                            <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" title="Vinnie Mostowy" class="avatar avatar-sm pull-up">
                                <img class="rounded-circle" src="//admin/img/avatars/5.png" alt="Avatar">
                            </li>
                            <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" title="Allen Rieske" class="avatar avatar-sm pull-up">
                                <img class="rounded-circle" src="/admin/img/avatars/12.png" alt="Avatar">
                            </li>
                            <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" title="Julee Rossignol" class="avatar avatar-sm pull-up">
                                <img class="rounded-circle" src="/admin/img/avatars/6.png" alt="Avatar">
                            </li>
                            <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" title="Kaith D'souza" class="avatar avatar-sm pull-up">
                                <img class="rounded-circle" src="/admin/img/avatars/3.png" alt="Avatar">
                            </li>
                            <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" title="John Doe" class="avatar avatar-sm pull-up">
                                <img class="rounded-circle" src="/admin/img/avatars/1.png" alt="Avatar">
                            </li>
                        </ul>
                    </div>
                    <div class="d-flex justify-content-between align-items-end mt-1">
                        <div class="role-heading">
                            <h4 class="mb-1">${role.role.name}</h4>
                            <a href="javascript:void(0);" class="role-edit-modal edit-role-btn" data-id="${role.role.id}"><span>Sửa</span></a>
                        </div>
                        <a href="javascript:void(0);" class="text-muted delete-role-btn" data-id="${role.role.id}"><i class="ti ti-trash ti-md"></i></a>
                    </div>
                </div>
            </div>
        </div>`;
    return html;
}
function generateAddRoleHtml() {
    const html = `<div class="col-xl-4 col-lg-6 col-md-6">
        <div class="card h-100">
            <div class="row h-100">
                <div class="col-sm-5">
                    <div class="d-flex align-items-end h-100 justify-content-center mt-sm-0 mt-3">
                        <img src="/admin/img/illustrations/add-new-roles.png" class="img-fluid mt-sm-4 mt-md-0" alt="add-new-roles" width="83">
                    </div>
                </div>
                <div class="col-sm-7">
                    <div class="card-body text-sm-end text-center ps-sm-0">
                        <button data-bs-target="#addRoleModal" data-bs-toggle="modal" class="btn btn-primary mb-2 text-nowrap add-new-role">Thêm vai trò</button>
                        <p class="mb-0 mt-1">Thêm mới nếu chưa có</p>
                    </div>
                </div>
            </div>
        </div>
    </div>`;
    return html;
}
// list permission
async function listPermissionAsync() {
    $.ajax({
        url: `${ApiServer}/api/permission/list`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            await showPermissionListAsync(data.data);
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await listPermissionAsync();
            }
        }
    });
}
async function showPermissionListAsync(permissions) {
    var html = "";
    html += generateSelectAllPermissionHtml();
    permissions.forEach((permission) => {

        html += generatePermissionHtml(permission);
    });
    $("#addRoleForm #addRolePermissionTable tbody").html(html);
    $("#editRoleForm #editRolePermissionTable tbody").html(html);
    await setSelectAllBtnClick();
}
function generatePermissionHtml(permission) {
    const html = `<tr>
        <td class="text-nowrap fw-medium">${permission.name} </td>
        <td>
            <div class="d-flex">
                <div class="form-check me-3 me-lg-5">
                    <input class="form-check-input" type="checkbox" name="rolePermissions" data-id="${permission.id}" />
                </div>
            </div>
        </td>
    </tr>`;
    return html;
}
function generateSelectAllPermissionHtml() {
    const html = `<tr>
        <td class="text-nowrap fw-medium">Truy cập như Admin <i class="ti ti-info-circle" data-bs-toggle="tooltip" data-bs-placement="top" title="Cho phép tất cả quyền truy cập"></i></td>
        <td>
            <div class="form-check">
                <input class="form-check-input" type="checkbox" name="selectAll" />
                <label class="form-check-label" for="addSelectAll">
                    Chọn tất cả
                </label>
            </div>
        </td>
    </tr>`;
    return html;
}
async function setSelectAllBtnClick() {
    
    $("#addRoleForm input[name=selectAll]").on("change", async function (e) {
        $("#addRoleForm input[name=rolePermissions]").prop("checked", $(this).is(':checked'));
    });
    $("#editRoleForm input[name=selectAll]").on("click", async function (e) {
        $("#editRoleForm input[name=rolePermissions]").prop("checked", $(this).is(':checked'));
    });
}

//add role
async function addRoleAsync() {
    var data = await getAddRoleInputAsync();
    $.ajax({
        url: `${ApiServer}/api/role/create`,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            await listRoleAsync(data.data);
            await closeAddRoleForm();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await addRoleAsync();
            }
        }
    });
}
async function closeAddRoleForm() {
    $("#addRoleForm button[type=reset]").click();
}
async function resetAddRoleForm() {
    cleanValidateField("#addRoleForm");
    $("#addRoleForm").trigger("reset");

}
async function getAddRoleInputAsync() {
    var permissionArray = new Array();
    $("#addRoleForm input[name=rolePermissions]:checked").each(function () {
        permissionArray.push($(this).data("id"));
    });
    console.log(permissionArray);
    return JSON.stringify({
        name: $("#addRoleForm input[name=addRoleName]").val(),
        permissionIds: permissionArray,
    });
}
//delete role
async function setDeleteRoleBtnClick() {
    $(".delete-role-btn").on("click", async function (e) {
        e.preventDefault();
        await questionDeleteRoleAsync($(this).data("id"));
    });
}
async function questionDeleteRoleAsync(id) {
    Swal.fire({
        title: "Xác nhận xóa?",
        text: "Dữ liệu sẽ không thể khôi phục!",
        icon: "warning",
        showCancelButton: !0,
        confirmButtonText: "Xóa",
        cancelButtonText: "Hủy",
        customClass: {
            confirmButton: "btn btn-primary me-3 waves-effect waves-light",
            cancelButton: "btn btn-label-secondary waves-effect waves-light"
        },
        buttonsStyling: !1
    }).then(async function (t) {
        if (t.value == true) {
            await deleteRoleAsync(id);
        }
    })
}
async function deleteRoleAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/role/delete/${id}`,
        type: 'DELETE',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            toastr.success(data.message);
            await listRoleAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await deleteRoleAsync(id);
            }
        }
    });
}
// edit role
async function setEditRoleBtnClick() {
    $(".edit-role-btn").on("click", async function (e) {
        e.preventDefault();
        await editRoleAsync($(this).data("id"));
    });
}
async function editRoleAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/role/get/${id}`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            await showEditRoleFormAsync(data.data);
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await editRoleAsync(id);
            }
        }
    });
}
async function showEditRoleFormAsync(data) {
    $("#editRoleModal").modal("show");
    $("#editRoleForm").data("id", data.role.id);
    $("#editRoleForm input[name=editRoleName]").val(data.role.name);
    data.permissions.forEach((permission) => {
        $(`#editRoleForm input[data-id=${permission.id}]`).prop("checked", true);
    });
}
async function resetEditRoleForm() {
    cleanValidateField("#editRoleForm");
    $("#editRoleForm").trigger("reset");
}
async function closeEditRoleFormAsync() {
    $("#editRoleForm button[type=reset]").click();
}
async function getEditRoleInputAsync() {
    var permissionArray = new Array();
    $("#editRoleForm input[name=rolePermissions]:checked").each(function () {
        permissionArray.push($(this).data("id"));
    });
    console.log(permissionArray);
    return JSON.stringify({
        name: $("#editRoleForm input[name=editRoleName]").val(),
        permissionIds: permissionArray,
    });
}
async function updateRoleAsync(id) {
    var data = await getEditRoleInputAsync();
    console.log(data);
    $.ajax({
        url: `${ApiServer}/api/role/update/${id}`,
        type: 'PUT',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            toastr.success(data.message);
            await listRoleAsync();
            await closeEditRoleFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await updateRoleAsync();
            }
        }
    });
}
