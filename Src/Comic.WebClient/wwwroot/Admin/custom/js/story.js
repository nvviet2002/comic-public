var addStoryDescriptionEditor;
var editStoryDescriptionEditor;
$(document).ready(async function () {
    await initStoryDatatableAsync();
    await initAddStoryFormAsync();
    await initEditStoryFormAsync();
    await initCategoriesForTwoFormAsync();
    //add
    $("#addStoryForm").on("submit", async function (e) {
        e.preventDefault();
        await addStoryAsync();
    });
    $("#addStoryBtn").on("click", async function (e) {
        e.preventDefault();
        console.log("resettt");
        await resetAddStoryFormAsync();
    });
    //update
    $("#editStoryForm").on("submit", async function (e) {
        e.preventDefault();
        const id = $(this).data("id");
        await updateStoryAsync(id);
    });
});

// list datatable
function generateActionBtnHtml(id) {
    var html = `<div class="dropdown">
        <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical"></i></button>
        <div class="dropdown-menu">
            <a class="dropdown-item edit-story-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-pencil me-1"></i> Sửa</a>
            <a class="dropdown-item detail-story-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-eye me-1"></i> Chi tiết</a>
            <a class="dropdown-item delete-story-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-trash me-1"></i> Xóa</a>
        </div>
    </div>`;
    return html;
}
async function initStoryDatatableAsync() {
    $("#storyDatatable").DataTable({
        ajax: {
            url: `${ApiServer}/api/story/list-datatable`,
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
                        order.column = "view";
                    } else if (order.column == 5) {
                        order.column = "updatedAt";
                    } else if (order.column == 6) {
                        order.column = "createdAt";
                    } else {
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
                    await resetStoryDatatableAsync();
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
            {
                'data': 'name', orderable: true, render: function (data, type, row, meta) {
                    return `<span class="text-overflow">${data}</span>`;
                }
            },
            {
                'data': 'categoryNames', orderable: false, render: function (data, type, row, meta) {
                    return `<span class="text-overflow">${data}</span>`;
                }
            },
            {
                'data': 'avatar', orderable: false, render: function (data, type, row, meta) {
                    return `<img src="${data}" heght="auto" width="60"/>`;
                }
            },
            
            { 'data': 'view' },
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
                                void 0 !== t.classList && t.classList.contains("story-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("story-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("story-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("story-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("story-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }]
        }, {
            text: '<span>Thêm mới</span>',
            className: "btn add-new btn-primary mb-3 mb-md-0 waves-effect waves-light",
            attr: {
                "id": "addStoryBtn",
                "data-bs-toggle": "modal",
                "data-bs-target": "#addStoryModal"
            }
        }],
        drawCallback: async function () {
            await setDeleteStoryBtnClickAsync();
            await setEditStoryBtnClickAsync();
            await setDetailStoryBtnClickAsync();
        },
    });
}
async function resetStoryDatatableAsync() {
    $('#storyDatatable').DataTable().ajax.reload();
}
//general
async function initCategoriesForTwoFormAsync() {
    $.ajax({
        url: `${ApiServer}/api/category/list`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        success: async function (data) {
            await showCategorySelectAsync(data.data);
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await initCategoriesForTwoFormAsync();
            }
        }
    });
}
async function showCategorySelectAsync(categories) {
    var categoriesHtml = "";
    categories.forEach(category => {
        categoriesHtml += `<option value="${category.id}">${category.name}</option>`;
    });
    $("#addStoryForm #addStoryCategoryIds").html(categoriesHtml);
    $("#editStoryForm #editStoryCategoryIds").html(categoriesHtml);
}
//add story
async function initAddStoryFormAsync() {
    ClassicEditor.create(document.querySelector('#addStoryDescription'))
        .then(newEditor => {
            addStoryDescriptionEditor = newEditor;
        })
        .catch(error => {
            console.error(error);
        });
    $("#addStoryForm #addStoryAvatar").on("change", async function (e) {
        await showAddStoryAvatarPreviewAsync();
    });

}
async function showAddStoryAvatarPreviewAsync() {

    var addStoryAvatar = $("#addStoryForm #addStoryAvatar");
    if (addStoryAvatar.prop("files")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#addStoryForm #addStoryAvatarPreview").attr('src', e.target.result);
        };

        reader.readAsDataURL(addStoryAvatar.prop("files")[0]);
    }
}
async function addStoryAsync() {
    var data = await getAddStoryInputAsync();
    console.log(data);
    $.ajax({
        url: `${ApiServer}/api/story/create`,
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
            await resetStoryDatatableAsync();
            await closeAddStoryFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await addStoryAsync();
            }
        }
    });
}
async function closeAddStoryFormAsync() {
    $("#addStoryForm button[type=reset]").click();
}
async function getAddStoryInputAsync() {
    var form = new FormData();
    form.append("name", $("#addStoryForm #addStoryName").val());
    form.append("otherName", $("#addStoryForm #addStoryOtherName").val());
    form.append("description", addStoryDescriptionEditor.getData());
    form.append("metaKeyword", $("#addStoryForm #addStoryMetaKeyword").val());
    form.append("metaDescription", $("#addStoryForm #addStoryMetaDescription").val());
    form.append("avatar", $("#addStoryForm #addStoryAvatar").prop("files")[0]);
    form.append("status", $("#addStoryForm #addStoryStatus").selectpicker('val'));
    form.append("isActived", $("#addStoryForm #addStoryIsActived").prop("checked"));
    form.append("hotFlag", $("#addStoryForm #addStoryHotFlag").prop("checked"));
    $("#addStoryForm #addStoryCategoryIds").val().forEach(categoryId => {
        form.append("categoryIds", categoryId);
    });
    return form;
}
async function resetAddStoryFormAsync() {
    cleanValidateField("#addStoryForm");
    $("#addStoryForm").trigger("reset");
    addStoryDescriptionEditor.setData("");
    $('#addStoryForm  #addStoryStatus').selectpicker("val", "New");
    $('#addStoryForm  #addStoryCategoryIds').val("").trigger("change");
    $('#addStoryForm  #addStoryIsActived').prop("checked", true);
    $('#addStoryForm  #addStoryHotFlag').prop("checked", false);
    $("#addStoryForm #addStoryAvatarPreview").attr('src', "");

}
//delete story
async function setDeleteStoryBtnClickAsync() {
    $(".delete-story-btn").on("click", async function (e) {
        e.preventDefault();
        await questionDeleteStoryAsync($(this).data("id"));
    });
}
async function questionDeleteStoryAsync(id) {
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
            await deleteStoryAsync(id);
        }
    })
}
async function deleteStoryAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/story/delete/${id}`,
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
            await resetStoryDatatableAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await deleteStoryAsync(id);
            }
        }
    });
}
//edit story
async function setEditStoryBtnClickAsync() {
    $(".edit-story-btn").on("click", async function (e) {
        e.preventDefault();
        await editStoryAsync($(this).data("id"));
    });
}
async function initEditStoryFormAsync() {
    ClassicEditor.create(document.querySelector('#editStoryDescription'))
        .then(newEditor => {
            editStoryDescriptionEditor = newEditor;
        })
        .catch(error => {
            console.error(error);
        });
    $("#editStoryForm #editStoryAvatar").on("change", async function (e) {
        await showEditStoryAvatarPreviewAsync();
    });


}
async function showEditStoryAvatarPreviewAsync() {

    var editStoryAvatar = $("#editStoryForm #editStoryAvatar");
    if (editStoryAvatar.prop("files")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#editStoryForm #editStoryAvatarPreview").attr('src', e.target.result);
        };

        reader.readAsDataURL(editStoryAvatar.prop("files")[0]);
    }
}
async function resetEditStoryFormAsync() {
    cleanValidateField("#editStoryForm");
    $("#editStoryForm").trigger("reset");
    editStoryDescriptionEditor.setData("");
    $('#editStoryForm  #editStoryStatus').selectpicker("val", "New");
    $('#editStoryForm  #editStoryCategoryIds').val("").trigger("change");
    $('#editStoryForm  #editStoryIsActived').prop("checked", true);
    $('#editStoryForm  #editStoryHotFlag').prop("checked", false);
    $("#editStoryForm #editStoryAvatarPreview").attr('src', "");
}
async function editStoryAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/story/get/${id}`,
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
            await showEditStoryFormAsync(data.data);

        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await editStoryAsync(id);
            }
        }
    });
}
async function showEditStoryFormAsync(story) {
    $("#editStoryModal").modal("show");
    await resetEditStoryFormAsync();
    $("#editStoryForm").data("id", story.id);
    $("#editStoryForm #editStoryName").val(story.name);
    $("#editStoryForm #editStoryOtherName").val(story.otherName);
    editStoryDescriptionEditor.setData(story.description);
    $("#editStoryForm #editStoryMetaKeyword").val(story.metaKeyword);
    $("#editStoryForm #editStoryMetaDescription").val(story.metaDescription);
    $("#editStoryForm #editStoryStatus").selectpicker('val', story.status);
    $("#editStoryForm #editStoryHotFlag").prop("checked", story.hotFlag);
    $('#editStoryForm #editStoryIsActived').prop("checked", story.isActived);
    $('#editStoryForm #editStoryAvatarPreview').attr("src", story.avatar);
    var categoryIds = [];
    story.categories.forEach(category => {
        categoryIds.push(category.id);
    });
    $("#editStoryForm #editStoryCategoryIds").val(categoryIds).trigger("change");
}
async function closeEditStoryFormAsync() {
    $("#editStoryForm button[type=reset]").click();
}
async function getEditStoryInputAsync() {
    var form = new FormData();
    form.append("name", $("#editStoryForm #editStoryName").val());
    form.append("otherName", $("#editStoryForm #editStoryOtherName").val());
    form.append("description", editStoryDescriptionEditor.getData());
    form.append("metaKeyword", $("#editStoryForm #editStoryMetaKeyword").val());
    form.append("metaDescription", $("#editStoryForm #editStoryMetaDescription").val());
    form.append("avatar", $("#editStoryForm #editStoryAvatar").prop("files")[0]);
    form.append("status", $("#editStoryForm #editStoryStatus").selectpicker('val'));
    form.append("isActived", $("#editStoryForm #editStoryIsActived").prop("checked"));
    form.append("hotFlag", $("#editStoryForm #editStoryHotFlag").prop("checked"));
    $("#editStoryForm #editStoryCategoryIds").val().forEach(categoryId => {
        form.append("categoryIds", categoryId);
    });
    return form;
}
async function updateStoryAsync(id) {
    var data = await getEditStoryInputAsync();
    $.ajax({
        url: `${ApiServer}/api/story/update/${id}`,
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
            await resetStoryDatatableAsync();
            await closeEditStoryFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await updateStoryAsync();
            }
        }
    });
}
//detail story
async function setDetailStoryBtnClickAsync() {
    $(".detail-story-btn").on("click", async function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        window.location.href = `${ClientServer}/admin/story/${id}/chapter`;
    });
}