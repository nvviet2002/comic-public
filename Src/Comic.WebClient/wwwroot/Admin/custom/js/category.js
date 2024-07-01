var addCategoryDescriptionEditor;
var editCategoryDescriptionEditor;
$(document).ready(async function () {
    await initCategoryDatatableAsync();
    await initAddCategoryFormAsync();
    await initEditCategoryFormAsync();
    //add
    $("#addCategoryForm").on("submit", async function (e) {
        e.preventDefault();
        await addCategoryAsync();
    });
    $("#addCategoryBtn").on("click", async function (e) {
        e.preventDefault();
        await resetAddCategoryFormAsync();
    });
    //update
    $("#editCategoryForm").on("submit", async function (e) {
        e.preventDefault();
        const id = $(this).data("id");
        await updateCategoryAsync(id);
    });
});

// list datatable
function generateActionBtnHtml(id) {
    var html = `<div class="dropdown">
        <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical"></i></button>
        <div class="dropdown-menu">
            <a class="dropdown-item edit-category-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-pencil me-1"></i> Sửa</a>
            <a class="dropdown-item delete-category-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-trash me-1"></i> Xóa</a>
        </div>
    </div>`;
    return html;
}
async function initCategoryDatatableAsync() {
    $("#categoryDatatable").DataTable({
        ajax: {
            url: `${ApiServer}/api/category/list-datatable`,
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
                    } else if (order.column == 4) {
                        order.column = "updatedAt";
                    } else if (order.column == 5) {
                        order.column = "createdAt";
                    }else {
                        order.column = "";
                    }
                });
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
                    await resetCategoryDatatableAsync();
                }
            },
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
            { 'data': 'name' },
            { 'data': 'type', orderable: false },
            {
                'data': 'description', orderable: false, render: function (data, type, row, meta) {
                    return `<span class="text-overflow">${data}</span>`;
                }
            },
            {
                'data': 'updatedAt', orderable: true, render: function (data, type, row, meta) {
                    return convertUTCToLocaleString(data);
                }
            },
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
            searchPlaceholder: "Nhập tên...",
            decimal: "",
            emptyTable: "Không có dữ liệu",
            info: "Hiển thị _START_ -> _END_ của _TOTAL_ dòng",
            infoEmpty: "Hiển thị 0 -> 0 của _TOTAL_ dòng",
            infoFiltered: "(Lọc từ _MAX_ tổng dòng)",
            loadingRecords: "Đang tải...",
            zeroRecords: "Không tìm thấy dữ liệu",
            paginate: {
                first: "Đầu",
                last: "Cuối",
                next: "Sau",
                previous: "Trước"
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
                                void 0 !== t.classList && t.classList.contains("category-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("category-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("category-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("category-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("category-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }]
        }, {
            text: '<span>Thêm mới</span>',
            className: "btn add-new btn-primary mb-3 mb-md-0 waves-effect waves-light",
            attr: {
                "id": "addCategoryBtn",
                "data-bs-toggle": "modal",
                "data-bs-target": "#addCategoryModal"
            }
        }],
        drawCallback: async function () {
            await setDeleteCategoryBtnClickAsync();
            await setEditCategoryBtnClickAsync();
        },
    });
}
async function resetCategoryDatatableAsync() {
    $('#categoryDatatable').DataTable().ajax.reload();
}
//add category
async function initAddCategoryFormAsync() {
    ClassicEditor.create(document.querySelector('#addCategoryDescription'))
        .then(newEditor => {
            addCategoryDescriptionEditor = newEditor;
        })
        .catch(error => {
            console.error(error);
        });
}
async function addCategoryAsync() {
    var data = await getAddCategoryInputAsync();
    console.log(data);
    $.ajax({
        url: `${ApiServer}/api/category/create`,
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
            await resetCategoryDatatableAsync();
            await closeAddCategoryFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await addCategoryAsync();
            }
        }
    });
}
async function closeAddCategoryFormAsync() {
    $("#addCategoryForm button[type=reset]").click();
}
async function getAddCategoryInputAsync() {
    var form = new FormData();
    form.append("name", $("#addCategoryForm #addCategoryName").val());
    form.append("description", addCategoryDescriptionEditor.getData());
    form.append("metaKeyword", $("#addCategoryForm #addCategoryMetaKeyword").val());
    form.append("metaDescription", $("#addCategoryForm #addCategoryMetaDescription").val());
    form.append("type", $("#addCategoryForm #addCategoryType").selectpicker('val'));
    form.append("isActived", $("#addCategoryForm #addCategoryIsActived").prop("checked"));
    return form;
}
async function resetAddCategoryFormAsync() {
    cleanValidateField("#addCategoryForm");
    $('#addCategoryForm  select[name=addCategoryType]').selectpicker("val","Category");
    $("#addCategoryForm").trigger("reset");
    addCategoryDescriptionEditor.setData("");
    $('#addCategoryForm  input[name=addCategoryIsActived]').prop("checked", true);
}
//delete category
async function setDeleteCategoryBtnClickAsync() {
    $(".delete-category-btn").on("click", async function (e) {
        e.preventDefault();
        await questionDeleteCategoryAsync($(this).data("id"));
    });
}
async function questionDeleteCategoryAsync(id) {
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
            await deleteCategoryAsync(id);
        }
    })
}
async function deleteCategoryAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/category/delete/${id}`,
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
            await resetCategoryDatatableAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await deleteCategoryAsync(id);
            }
        }
    });
}
//edit category
async function setEditCategoryBtnClickAsync() {
    $(".edit-category-btn").on("click", async function (e) {
        e.preventDefault();
        await editCategoryAsync($(this).data("id"));
    });
}
async function initEditCategoryFormAsync() {
    ClassicEditor.create(document.querySelector('#editCategoryDescription'))
        .then(newEditor => {
            editCategoryDescriptionEditor = newEditor;
        })
        .catch(error => {
            console.error(error);
        });


}
async function resetEditCategoryFormAsync() {
    cleanValidateField("#editCategoryForm");
    $('#editCategoryForm  select[name=editCategoryType]').selectpicker("val", "Category");
    $("#editCategoryForm").trigger("reset");
    editCategoryDescriptionEditor.setData("");
    $('#editCategoryForm  input[name=editCategoryIsActived]').prop("checked", true);
}
async function editCategoryAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/category/get/${id}`,
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
            await showEditCategoryFormAsync(data.data);

        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await editCategoryAsync(id);
            }
        }
    });
}
async function showEditCategoryFormAsync(category) {
    $("#editCategoryModal").modal("show");
    await resetEditCategoryFormAsync();
    $("#editCategoryForm").data("id", category.id);
    $("#editCategoryForm #editCategoryName").val(category.name);
    editCategoryDescriptionEditor.setData(category.description);
    $("#editCategoryForm #editCategoryMetaKeyword").val(category.metaKeyword);
    $("#editCategoryForm #editCategoryMetaDescription").val(category.metaDescription);
    $('#editCategoryForm #editCategoryType').selectpicker("val", category.type);
    $('#editCategoryForm #editCategoryIsActived').prop("checked", category.isActived);
}
async function closeEditCategoryFormAsync() {
    $("#editCategoryForm button[type=reset]").click();
}
async function getEditCategoryInputAsync() {
    var form = new FormData();
    form.append("name", $("#editCategoryForm #editCategoryName").val());
    form.append("description", editCategoryDescriptionEditor.getData());
    form.append("metaKeyword", $("#editCategoryForm #editCategoryMetaKeyword").val());
    form.append("metaDescription", $("#editCategoryForm #editCategoryMetaDescription").val());
    form.append("type", $("#editCategoryForm #editCategoryType").selectpicker('val'));
    form.append("isActived", $("#editCategoryForm #editCategoryIsActived").prop("checked"));
    return form;
}
async function updateCategoryAsync(id) {
    var data = await getEditCategoryInputAsync();
    $.ajax({
        url: `${ApiServer}/api/category/update/${id}`,
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
            await resetCategoryDatatableAsync();
            await closeEditCategoryFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await updateCategoryAsync();
            }
        }
    });
}