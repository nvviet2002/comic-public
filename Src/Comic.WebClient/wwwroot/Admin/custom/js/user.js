
$(document).ready(async function () {
    await initUserDatatableAsync();
    await initAddUserFormAsync();
    await initEditUserFormAsync();
    await initRolesForTwoFormAsync();

    //add
    $("#addUserForm").on("submit", async function (e) {
        e.preventDefault();
        await addUserAsync();
    });
    $("#addUserBtn").on("click", async function (e) {
        e.preventDefault();
        await resetAddUserFormAsync();

    });
    //update
    $("#editUserForm").on("submit", async function (e) {
        e.preventDefault();
        const id = $(this).data("id");
        console.log("id: " + id);
        await updateUserAsync(id);
    });
});

// list datatable
function generateActionBtnHtml(id) {
    var html = `<div class="dropdown">
        <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical"></i></button>
        <div class="dropdown-menu">
            <a class="dropdown-item edit-user-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-pencil me-1"></i> Sửa</a>
            <a class="dropdown-item delete-user-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-trash me-1"></i> Xóa</a>
        </div>
    </div>`;
    return html;
}
async function initUserDatatableAsync() {
    $("#userDatatable").dataTable({
        ajax: {
            url: `${ApiServer}/api/user/list-datatable`,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            crossDomain: true,
            xhrFields: {
                withCredentials: true
            },
            data: function (data) {
                data.order.forEach(order => {
                    if (order.column == 1) {
                        order.column = "name";
                    } else {
                        order.column = "";
                    }
                });
                console.log(data);
                return JSON.stringify(data);
            },
            dataSrc: function (data) {
                data.draw = data.data.draw;
                data.recordsTotal = data.data.recordsTotal;
                data.recordsFiltered = data.data.recordsFiltered;
                return data.data.data;
            },
            error: async function (httpObject, error) {
                const result = await handleErrorReponseAsync(httpObject);
                if (result == true) {
                    await resetUserDatatableAsync();
                }
            }
        },
        processing: true,
        serverSide: true,
        pagingType: 'full_numbers',
        lengthMenu: [10, 25, 50, 100],
        order: [[1, 'asc']],
        columns: [
            {
                'data': 'id', orderable: false, render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { 'data': 'email' },
            { 'data': 'name' },
            {
                'data': 'birthday', orderable: false, render: function (data, type, row, meta) {
                    return convertUTCToLocaleDateString(data);
                }
            },
            { 'data': 'point' },
            { 'data': 'roleNames', orderable: false },
            {
                'data': 'createdAt', orderable: true, render: function (data, type, row, meta) {
                    return convertUTCToLocaleString(data);
                }
            },
            {
                'data': 'isActived', orderable: false, render: function (data, type, row, meta) {
                    return generateActiveHtml(data);
                }
            },
            {
                'data': 'id', orderable: false, render: function (data, type, row, meta) {
                    return generateActionBtnHtml(data);
                }
            },
        ],
        dom: '<"row me-2"<"col-md-2"<"me-3"l>><"col-md-10"<"dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-end flex-md-row flex-column mb-3 mb-md-0"fB>>>t<"row mx-2"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
        language: {
            lengthMenu: "_MENU_",
            search: "",
            searchPlaceholder: "Nhập email...",
            "decimal": "",
            "emptyTable": "Không có dữ liệu",
            "info": "Hiển thị _START_ -> _END_ của _TOTAL_ dòng",
            "infoEmpty": "Hiển thị 0 -> 0 của _TOTAL_ dòng",
            "infoFiltered": "(Lọc từ _MAX_ tổng dòng)",
            "loadingRecords": "Đang tải...",
            "zeroRecords": "Không tìm thấy dữ liệu",
            "paginate": {
                "first": "Đầu",
                "last": "Cuối",
                "next": "Sau",
                "previous": "Trước"
            },
        },
        buttons: [{
            extend: "collection",
            className: "btn btn-label-secondary dropdown-toggle mx-3 waves-effect waves-light",
            text: '<i class="ti ti-screen-share me-1 ti-xs"></i>Export',
            buttons: [{
                extend: "print",
                text: '<i class="ti ti-printer me-2" ></i>Print',
                className: "dropdown-item",
                exportOptions: {
                    columns: [1, 2, 3, 4, 5],
                    format: {
                        body: function (e, t, a) {
                            var s;
                            return e.length <= 0 ? e : (e = $.parseHTML(e), s = "", $.each(e, function (e, t) {
                                void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                },
                customize: function (e) {
                    $(e.document.body).css("color", s).css("border-color", t).css("background-color", a), $(e.document.body).find("table").addClass("compact").css("color", "inherit").css("border-color", "inherit").css("background-color", "inherit")
                }
            }, {
                extend: "csv",
                text: '<i class="ti ti-file-text me-2" ></i>Csv',
                className: "dropdown-item",
                exportOptions: {
                    columns: [1, 2, 3, 4, 5],
                    format: {
                        body: function (e, t, a) {
                            var s;
                            return e.length <= 0 ? e : (e = $.parseHTML(e), s = "", $.each(e, function (e, t) {
                                void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }, {
                extend: "excel",
                text: '<i class="ti ti-file-spreadsheet me-2"></i>Excel',
                className: "dropdown-item",
                exportOptions: {
                    columns: [1, 2, 3, 4, 5],
                    format: {
                        body: function (e, t, a) {
                            var s;
                            return e.length <= 0 ? e : (e = $.parseHTML(e), s = "", $.each(e, function (e, t) {
                                void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }, {
                extend: "pdf",
                text: '<i class="ti ti-file-code-2 me-2"></i>Pdf',
                className: "dropdown-item",
                exportOptions: {
                    columns: [1, 2, 3, 4, 5],
                    format: {
                        body: function (e, t, a) {
                            var s;
                            return e.length <= 0 ? e : (e = $.parseHTML(e), s = "", $.each(e, function (e, t) {
                                void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }, {
                extend: "copy",
                text: '<i class="ti ti-copy me-2" ></i>Copy',
                className: "dropdown-item",
                exportOptions: {
                    columns: [1, 2, 3, 4, 5],
                    format: {
                        body: function (e, t, a) {
                            var s;
                            return e.length <= 0 ? e : (e = $.parseHTML(e), s = "", $.each(e, function (e, t) {
                                void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }]
        }, {
            text: '<span>Thêm mới</span>',
            className: "btn add-new btn-primary mb-3 mb-md-0 waves-effect waves-light",
            attr: {
                "id": "addUserBtn",
                "data-bs-toggle": "modal",
                "data-bs-target": "#addUserModal"
            }
        }],
        drawCallback: async function () {
            await setDeleteUserBtnClickAsync();
            await setEditUserBtnClickAsync();
        }
    });
}
async function resetUserDatatableAsync() {
    $('#userDatatable').DataTable().ajax.reload();
}
//general
async function initRolesForTwoFormAsync() {
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
        success: async function (data) {
            await showRoleSelectAsync(data.data);
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await initRolesForTwoFormAsync();
            }
        }
    });
}
async function showRoleSelectAsync(roles) {
    var rolesHtml = "";
    roles.forEach(role => {
        rolesHtml += `<option value="${role.role.id}">${role.role.name}</option>`;
    });
    $("#addUserForm select[name=addUserRoleIds]").html(rolesHtml);
    $("#editUserForm select[name=editUserRoleIds]").html(rolesHtml);
    $('#addUserForm  .selectpicker').selectpicker('refresh');
    $('#editUserForm  .selectpicker').selectpicker('refresh');
}
//add User
async function initAddUserFormAsync() {
    $("#addUserForm #addUserBirthday").datepicker({
        todayHighlight: true,
        format: "dd/mm/yyyy",
        setDate: new Date(),
        autoclose: true,
        orientation: isRtl ? "auto right" : "auto left"
    });

    var addUserPhoneNumber = $("#addUserPhoneNumber");
    new Cleave(addUserPhoneNumber, {
        phone: true,
        phoneRegionCode: "VN",
    });
    var addUserPoint = $("#addUserPoint");
    new Cleave(addUserPoint, {
        numeral: true,
        numeralThousandsGroupStyle: "thousand",
        numeralDecimalScale: 1,
    });
    $("#addUserForm input[name=addUserAvatar]").on("change", async function (e) {
        await showAddUserAvatarPreviewAsync();
    });


}
async function showAddUserAvatarPreviewAsync() {
    
    var addUserAvatar = $("#addUserForm input[name = addUserAvatar]");
    console.log(addUserAvatar.prop("files")[0]);
    if (addUserAvatar.prop("files")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#addUserForm #addUserAvatarPreview").attr('src', e.target.result);
        };
        
        reader.readAsDataURL(addUserAvatar.prop("files")[0]);
    }
}
async function addUserAsync() {
    var data = await getAddUserInputAsync();
    console.log(data);
    $.ajax({
        url: `${ApiServer}/api/user/create`,
        type: 'POST',
        processData: false,
        contentType: false,
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
            await resetUserDatatableAsync();
            await closeAddUserFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await addUserAsync();
            }
        }
    });
}
async function closeAddUserFormAsync() {
    $("#addUserForm button[type=reset]").click();
}
async function getAddUserInputAsync() {
    var form = new FormData();
    form.append("email", $("#addUserForm input[name=addUserEmail]").val());
    form.append("password", $("#addUserForm input[name=addUserPassword]").val());
    form.append("name", $("#addUserForm input[name=addUserName]").val());
    form.append("phoneNumber", $("#addUserForm input[name=addUserPhoneNumber]").val());
    form.append("birthday", convertLocaleDateToISO($("#addUserForm input[name=addUserBirthday]").datepicker("getDate")));
    form.append("point", $("#addUserForm input[name=addUserPoint]").val());
    form.append("isActived", $("#addUserForm input[name=addUserIsActived]").prop("checked"));
    form.append("avatar", $("#addUserForm input[name=addUserAvatar]").prop("files")[0]);
    $("#addUserForm select[name=addUserRoleIds]").selectpicker('val').forEach(roleId => {
        form.append("roleIds", roleId);
    });
    return form;
}
async function resetAddUserFormAsync() {
    cleanValidateField("#addUserForm");
    $('#addUserForm  .selectpicker').selectpicker('deselectAll');
    $("#addUserForm").trigger("reset");
    $('#addUserForm  input[name=addUserIsActived]').prop("checked", true);
    $("#addUserForm #addUserAvatarPreview").attr('src', "");
}

//delete User
async function setDeleteUserBtnClickAsync() {
    $(".delete-user-btn").on("click", async function (e) {
        e.preventDefault();
        await questionDeleteUserAsync($(this).data("id"));
    });
}
async function questionDeleteUserAsync(id) {
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
            await deleteUserAsync(id);
        }
    })
}
async function deleteUserAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/user/delete/${id}`,
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
            await resetUserDatatableAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await deleteUserAsync(id);
            }
        }
    });
}

// edit User
async function setEditUserBtnClickAsync() {
    $(".edit-user-btn").on("click", async function (e) {
        e.preventDefault();
        await editUserAsync($(this).data("id"));
    });
}
async function initEditUserFormAsync() {
    $("#editUserForm #editUserBirthday").datepicker({
        todayHighlight: true,
        format: "dd/mm/yyyy",
        setDate: new Date(),
        autoclose: true,
        orientation: isRtl ? "auto right" : "auto left"
    });

    var addUserPhoneNumber = $("#editUserPhoneNumber");
    new Cleave(addUserPhoneNumber, {
        phone: true,
        phoneRegionCode: "VN",
    });
    var addUserPoint = $("#editUserPoint");
    new Cleave(addUserPoint, {
        numeral: true,
        numeralThousandsGroupStyle: "thousand",
        numeralDecimalScale: 1,
    });
    $("#editUserForm input[name=editUserAvatar]").on("change", async function (e) {
        await showEditUserAvatarPreviewAsync();
    });


}
async function showEditUserAvatarPreviewAsync() {

    var editUserAvatar = $("#editUserForm input[name=editUserAvatar]");
    console.log(editUserAvatar.prop("files")[0]);
    if (editUserAvatar.prop("files")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#editUserForm #editUserAvatarPreview").attr('src', e.target.result);
        };

        reader.readAsDataURL(editUserAvatar.prop("files")[0]);
    }
}
async function resetEditUserFormAsync() {
    cleanValidateField("#editUserForm");
    $('#editUserForm  .selectpicker').selectpicker('deselectAll');
    $("#editUserForm").trigger("reset");
    $('#editUserForm  input[name=editUserIsActived]').prop("checked", true);
    $("#editUserForm #editUserAvatarPreview").attr('src', "");
}
async function editUserAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/user/get/${id}`,
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
            await showEditUserFormAsync(data.data);
            
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await editUserAsync(id);
            }
        }
    });
}
async function showEditUserFormAsync(user) {
    $("#editUserModal").modal("show");
    await resetEditUserFormAsync();
    console.log("user: "+user);
    $("#editUserForm").data("id", user.id);
    $("#editUserForm input[name=editUserEmail]").val(user.email);
    $("#editUserForm input[name=editUserPassword]").val();
    $("#editUserForm input[name=editUserName]").val(user.name);
    $("#editUserForm input[name=editUserPhoneNumber]").val(user.phoneNumber);
    $("#editUserForm input[name=editUserBirthday]").datepicker("setDate", convertUTCToLocalDate(user.birthday));
    $("#editUserForm input[name=editUserPoint]").val(user.point);
    $("#editUserForm input[name=editUserIsActived]").prop("checked",user.isActived);
    $("#editUserForm #editUserAvatarPreview").attr("src", user.avatar);
    var roleIds = [];
    user.roles.forEach(role => {
        roleIds.push(role.id);
    });
    $('#editUserForm select[name=editUserRoleIds]').selectpicker('val', roleIds);
}
async function closeEditUserFormAsync() {
    $("#editUserForm button[type=reset]").click();
}
async function getEditUserInputAsync() {
    var form = new FormData();
    form.append("email", $("#editUserForm input[name=editUserEmail]").val());
    form.append("password", $("#editUserForm input[name=editUserPassword]").val());
    form.append("name", $("#editUserForm input[name=editUserName]").val());
    form.append("phoneNumber", $("#editUserForm input[name=editUserPhoneNumber]").val());
    form.append("birthday", convertLocaleDateToISO($("#editUserForm input[name=editUserBirthday]").datepicker("getDate")));
    form.append("point", $("#editUserForm input[name=editUserPoint]").val());
    form.append("isActived", $("#editUserForm input[name=editUserIsActived]").prop("checked"));
    form.append("avatar", $("#editUserForm input[name=editUserAvatar]").prop("files")[0]);
    $("#editUserForm select[name=editUserRoleIds]").selectpicker('val').forEach(roleId => {
        form.append("roleIds", roleId);
    });
    return form;
}
async function updateUserAsync(id) {
    var data = await getEditUserInputAsync();
    $.ajax({
        url: `${ApiServer}/api/user/update/${id}`,
        type: 'PUT',
        processData: false,
        contentType: false,
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
            await resetUserDatatableAsync();
            await closeEditUserFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await updateUserAsync();
            }
        }
    });
}


