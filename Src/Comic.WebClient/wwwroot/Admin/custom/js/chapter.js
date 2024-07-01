
var addChapterViewCleave;
var editChapterViewCleave;
var addChapterIndexCleave;
var editChapterIndexCleave;
$(document).ready(async function () {
    await initChapterDatatableAsync();
    await initAddChapterFormAsync();
    await initEditChapterFormAsync();
    //add
    $("#addChapterForm").on("submit", async function (e) {
        e.preventDefault();
        await addChapterAsync();
    });
    $("#addChapterBtn").on("click", async function (e) {
        e.preventDefault();
        await resetAddChapterFormAsync();
    });
    //update
    $("#editChapterForm").on("submit", async function (e) {
        e.preventDefault();
        const id = $(this).data("id");
        await updateChapterAsync(id);
    });
});

// list datatable
function generateActionBtnHtml(id) {
    var html = `<div class="dropdown">
        <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical"></i></button>
        <div class="dropdown-menu">
            <a class="dropdown-item edit-chapter-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-pencil me-1"></i> Sửa</a>
            <a class="dropdown-item delete-chapter-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-trash me-1"></i> Xóa</a>
        </div>
    </div>`;
    return html;
}
async function initChapterDatatableAsync() {
    $("#chapterDatatable").DataTable({
        ajax: {
            url: `${ApiServer}/api/story/${storyId}/chapter/list-datatable`,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            crossDomain: true,
            xhrFields: {
                withCredentials: true
            },
            data: function (data) {
                data.order.forEach(order => {
                    if (order.column == 1) {
                        order.column = "index";
                    } else if (order.column == 2) {
                        order.column = "name";
                    } else if (order.column == 4) {
                        order.column = "view";
                    } else if (order.column == 5) {
                        order.column = "comment";
                    } else if (order.column == 6) {
                        order.column = "updatedAt";
                    } else if (order.column == 7) {
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
                    await resetChapterDatatableAsync();
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
            { 'data': 'index' },
            {
                'data': 'name', orderable: true, render: function (data, type, row, meta) {
                    return `<span class="text-overflow">${data}</span>`;
                }
            },
            {
                'data': 'title', orderable: false, render: function (data, type, row, meta) {
                    return `<span class="text-overflow">${data}</span>`;
                }
            },

            { 'data': 'view' },
            { 'data': 'comment' },
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
                                void 0 !== t.classList && t.classList.contains("chapter-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("chapter-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("chapter-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("chapter-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
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
                                void 0 !== t.classList && t.classList.contains("chapter-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                            }), s)
                        }
                    }
                }
            }]
        }, {
            text: '<span>Thêm mới</span>',
            className: "btn add-new btn-primary mb-3 mb-md-0 waves-effect waves-light",
            attr: {
                "id": "addChapterBtn",
                "data-bs-toggle": "modal",
                "data-bs-target": "#addChapterModal"
            }
        }],
        drawCallback: async function () {
            await setDeleteChapterBtnClickAsync();
            await setEditChapterBtnClickAsync();
        },
    });
}
async function resetChapterDatatableAsync() {
    $('#chapterDatatable').DataTable().ajax.reload();
}
//add chapter
async function initAddChapterFormAsync() {
    var addChapterView = $("#addChapterView");
    addChapterViewCleave = new Cleave(addChapterView, {
        numeral: true,
        numeralThousandsGroupStyle: "thousand",
        numeralDecimalScale: 0,
    });
    var addChapterIndex = $("#addChapterIndex");
    addChapterIndexCleave = new Cleave(addChapterIndex, {
        numeral: true,
        numeralThousandsGroupStyle: "thousand",
        numeralDecimalScale: 0,
    });
    $("#addChapterIndex").on("keyup paste change", async function (e) {
        $("#addChapterName").val(`Chapter ${addChapterIndexCleave.getRawValue()}`);
    });
    $("#addChapterForm #addChapterImages").on("change", async function (e) {
        await showAddChapterImagesPreviewAsync();
    });

}
async function showAddChapterImagesPreviewAsync() {
    var addChapterImages = $("#addChapterForm #addChapterImages");
    $("#addChapterForm #addChapterImagesPreview").html("");
    if (addChapterImages.prop("files")) {
        addChapterImages.prop("files").forEach(function(file) {
            console.log(URL.createObjectURL(file));
            var imgHtml = `<img src="${URL.createObjectURL(file) }" alt="Hình ảnh" height="auto" width="5%" />`
            $("#addChapterForm #addChapterImagesPreview").append(imgHtml);
        });
    }
}
async function addChapterAsync() {
    var data = await getAddChapterInputAsync();
    $.ajax({
        url: `${ApiServer}/api/story/${storyId}/chapter/create`,
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
            await resetChapterDatatableAsync();
            await closeAddChapterFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await addChapterAsync();
            }
        }
    });
}
async function closeAddChapterFormAsync() {
    $("#addChapterForm button[type=reset]").click();
}
async function getAddChapterInputAsync() {
    var form = new FormData();
    form.append("name", $("#addChapterForm #addChapterName").val());
    form.append("title", $("#addChapterForm #addChapterTitle").val());
    form.append("metaKeyword", $("#addChapterForm #addChapterMetaKeyword").val());
    form.append("metaDescription", $("#addChapterForm #addChapterMetaDescription").val());
    form.append("view", addChapterViewCleave.getRawValue());
    form.append("index", addChapterIndexCleave.getRawValue());
    form.append("status", $("#addChapterForm #addChapterStatus").selectpicker('val'));
    form.append("isActived", $("#addChapterForm #addChapterIsActived").prop("checked"));
    form.append("hotFlag", $("#addChapterForm #addChapterHotFlag").prop("checked"));
    $("#addChapterForm #addChapterImages").prop("files").forEach(file => {
        form.append("images", file);
    });
    return form;
}
async function resetAddChapterFormAsync() {
    cleanValidateField("#addChapterForm");
    $("#addChapterForm").trigger("reset");
    $('#addChapterForm  #addChapterStatus').selectpicker("val", "Full");
    $('#addChapterForm  #addChapterIsActived').prop("checked", true);
    $('#addChapterForm  #addChapterHotFlag').prop("checked", false);
    $("#addChapterForm #addChapterImagesPreview").html("");

}
//delete chapter
async function setDeleteChapterBtnClickAsync() {
    $(".delete-chapter-btn").on("click", async function (e) {
        e.preventDefault();
        await questionDeleteChapterAsync($(this).data("id"));
    });
}
async function questionDeleteChapterAsync(id) {
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
            await deleteChapterAsync(id);
        }
    })
}
async function deleteChapterAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/story/${storyId}/chapter/delete/${id}`,
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
            await resetChapterDatatableAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await deleteChapterAsync(id);
            }
        }
    });
}
//edit chapter
async function setEditChapterBtnClickAsync() {
    $(".edit-chapter-btn").on("click", async function (e) {
        e.preventDefault();
        await editChapterAsync($(this).data("id"));
    });
}
async function initEditChapterFormAsync() {
    var editChapterView = $("#editChapterView");
    editChapterViewCleave = new Cleave(editChapterView, {
        numeral: true,
        numeralThousandsGroupStyle: "thousand",
        numeralDecimalScale: 0,
    });
    var editChapterIndex = $("#editChapterIndex");
    editChapterIndexCleave = new Cleave(editChapterIndex, {
        numeral: true,
        numeralThousandsGroupStyle: "thousand",
        numeralDecimalScale: 0,
    });
    $("#editChapterIndex").on("keyup paste change", async function (e) {
        $("#editChapterName").val(`Chapter ${editChapterIndexCleave.getRawValue()}`);
    });
    $("#editChapterForm #editChapterImages").on("change", async function (e) {
        await showEditChapterImagesPreviewAsync();
    });


}
async function showEditChapterImagesPreviewAsync(imagePaths = null) {
    console.log(imagePaths);
    $("#editChapterForm #editChapterImagesPreview").html("");
    if (imagePaths == null) {
        var editChapterImages = $("#editChapterForm #editChapterImages");
        if (editChapterImages.prop("files")) {
            editChapterImages.prop("files").forEach(file => {
                var imgHtml = `<img src="${URL.createObjectURL(file)}" alt="Hình ảnh" height="auto" width="5%" />`
                $("#editChapterForm #editChapterImagesPreview").append(imgHtml);
            });
        }
    } else {
        imagePaths.forEach(path => {
            var imgHtml = `<img src="${path}" alt="Hình ảnh" height="auto" width="5%" />`
            $("#editChapterForm #editChapterImagesPreview").append(imgHtml);
        });
    }
   
}
async function resetEditChapterFormAsync() {

    cleanValidateField("#editChapterForm");
    $("#editChapterForm").trigger("reset");
    $('#editChapterForm  #editChapterStatus').selectpicker("val", "Full");
    $('#editChapterForm  #editChapterIsActived').prop("checked", true);
    $('#editChapterForm  #editChapterHotFlag').prop("checked", false);
    $("#editChapterForm #editChapterImagesPreview").html("");
}
async function editChapterAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/story/${storyId}/chapter/get/${id}`,
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
            await showEditChapterFormAsync(data.data);

        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await editChapterAsync(id);
            }
        }
    });
}
async function showEditChapterFormAsync(chapter) {
    $("#editChapterModal").modal("show");
    await resetEditChapterFormAsync();
    $("#editChapterForm").data("id", chapter.id);
    $("#editChapterForm #editChapterName").val(chapter.name);
    $("#editChapterForm #editChapterTitle").val(chapter.title);
    $("#editChapterForm #editChapterMetaKeyword").val(chapter.metaKeyword);
    $("#editChapterForm #editChapterMetaDescription").val(chapter.metaDescription);
    $("#editChapterForm #editChapterView").val(chapter.view);
    $("#editChapterForm #editChapterIndex").val(chapter.index);
    $("#editChapterForm #editChapterStatus").selectpicker('val', chapter.status);
    $("#editChapterForm #editChapterHotFlag").prop("checked", chapter.hotFlag);
    $('#editChapterForm #editChapterIsActived').prop("checked", chapter.isActived);
    await showEditChapterImagesPreviewAsync(chapter.images);
}
async function closeEditChapterFormAsync() {
    $("#editChapterForm button[type=reset]").click();
}
async function getEditChapterInputAsync() {
    var form = new FormData();
    form.append("name", $("#editChapterForm #editChapterName").val());
    form.append("title", $("#editChapterForm #editChapterTitle").val());
    form.append("metaKeyword", $("#editChapterForm #editChapterMetaKeyword").val());
    form.append("metaDescription", $("#editChapterForm #editChapterMetaDescription").val());
    form.append("view", editChapterViewCleave.getRawValue());
    form.append("index", editChapterIndexCleave.getRawValue());
    form.append("status", $("#editChapterForm #editChapterStatus").selectpicker('val'));
    form.append("isActived", $("#editChapterForm #editChapterIsActived").prop("checked"));
    form.append("hotFlag", $("#editChapterForm #editChapterHotFlag").prop("checked"));
    $("#editChapterForm #editChapterImages").prop("files").forEach(file => {
        form.append("images", file);
    });
    return form;
}
async function updateChapterAsync(id) {
    var data = await getEditChapterInputAsync();
    $.ajax({
        url: `${ApiServer}/api/story/${storyId}/chapter/update/${id}`,
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
            await resetChapterDatatableAsync();
            await closeEditChapterFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await updateChapterAsync();
            }
        }
    });
}