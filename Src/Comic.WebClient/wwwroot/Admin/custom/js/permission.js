$(document).ready(async function () {
    await initPermissionDatatableAsync();
    //await listPermissionAsync();
    //await setPageCountAndSearchTermChangeAsync();
    //add
    $("#addPermissionForm").on("submit", async function (e) {
        e.preventDefault();
        await addPermissionAsync();
    });
    $("#addPermissionModal").on("show.bs.modal", async function (e) {
        cleanValidateField("#addPermissionForm");
    });
    //update
    $("#editPermissionForm").on("submit", async function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        await updatePermissionAsync(id);
    });
    $("#editPermissionModal").on("show.bs.modal", async function (e) {
        cleanValidateField("#editPermissionForm");
    });


});

////list Permission
//async function listPermissionAsync() {
//    var data = await getPaginateInputAsync();
//    $.ajax({
//        url: `${ApiServer}/api/permission/list-paginate`,
//        type: 'GET',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: data,
//        crossDomain: true,
//        xhrFields: {
//            withCredentials: true
//        },
//        beforeSend: function () {
//            blockPage();
//        },
//        success: async function (data) {
//            await showPermissionListAsync(data.data.items);
//            await showPaginationAsync(data.data);
//        },
//        complete: function () {
//            unblockPage();
//        },
//        error: async function (httpObject, error) {
//            const result = await handleErrorReponseAsync(httpObject);
//            if (result == true) {
//                await listPermissionAsync();
//            }
//        }
//    });
//}
//async function showPermissionListAsync(permissions) {
//    var html = "";
//    permissions.forEach((permission, key) => {

//        html += generatePermissionHtml(key+1, permission);
//    });
//    $("#permissionTable tbody").html(html);
//    await setDeletePermissionBtnClickAsync();
//    await setEditPermissionBtnClickAsync();
//}
//function generatePermissionHtml(index, permission) {
//    const html = ` <tr>
//                        <td><input type="checkbox"/></td>
//                        <td>${index}</td>
//                        <td>${permission.name} </td>
//                        <td>${generateActiveHtml(permission.isActived)}</td>
//                        <td>
//                            <div class="dropdown">
//                                <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical"></i></button>
//                                <div class="dropdown-menu">
//                                    <a class="dropdown-item edit-permission-btn" href="javascript:void(0);" data-id="${permission.id}"><i class="ti ti-pencil me-1"></i> Sửa</a>
//                                    <a class="dropdown-item delete-permission-btn" href="javascript:void(0);" data-id="${permission.id}"><i class="ti ti-trash me-1"></i> Xóa</a>
//                                </div>
//                            </div>
//                        </td>
//                    </tr>`;
//    return html;
//}

////pagination
//async function setPageCountAndSearchTermChangeAsync() {
//    $("#pageSize").on("change", async function (e) {
//        await listPermissionAsync();
//    });
//    $("#searchTerm").on("change", async function (e) {
//        await listPermissionAsync();
//    });
//}
//async function setPaginateChangeAsync() {
//    $("#pagination .page-link").on("click", async function (e) {
//        $("#pagination .page-link").removeClass("active");
//        $(this).addClass("active");
//        await listPermissionAsync();
//    });
//}
//async function showPaginationAsync(data) {
//    $("#pageCount").html(`Hiển thị ${data.pageCount} / ${data.totalCount}`);
//    $("#pagination").html(generatePaginationHtml(data));
//    await setPaginateChangeAsync();
//}
//async function getPaginateInputAsync() {
//    return {
//        pageSize: $("#pageSize").val()??10,
//        pageNumber: $("#pagination .active").data("page")??1,
//        searchTerm: $("#searchTerm").val()??"",
//    };
//}
//function generatePaginationHtml(data) {
//    var html = ` <li class="page-item previous ${data.pagePrevious == data.pageNumber ? "disabled" : ""}">
//                            <a href="javascript:void(0);" class="page-link" data-page="${data.pagePrevious}">Trước</a>
//                        </li>`;
//    for (let i = 1; i <= data.totalPages; i++) {
//        html += `<li class="page-item" >
//                    <a href="javascript:void(0);" class="page-link  ${data.pageNumber == i ? "active" : ""}" data-page="${i}">${i}</a>
//                </li >`;
//    }
//    html += `<li class="page-item next ${data.pageNext == data.pageNumber ? "disabled" : ""}" >
//                <a href="javascript:void(0);" class="page-link" data-page="${data.pageNext}">Sau</a>
//            </li >`;
                
//    return html;
//}

//add Permission
async function addPermissionAsync() {
    var data = await getAddPermissionInputAsync();
    console.log(data);
    $.ajax({
        url: `${ApiServer}/api/permission/create`,
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
            toastr.success(data.message);
            await resetPermissionDatatableAsync();
            await closeAddPermissionFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await addPermissionAsync();
            }
        }
    });
}
async function closeAddPermissionFormAsync() {
    $("#addPermissionForm button[type=reset]").click();
}
async function getAddPermissionInputAsync() {
    return JSON.stringify({
        name: $("#addPermissionForm input[name=addPermissionName]").val(),
    });
}

//delete Permission
async function setDeletePermissionBtnClickAsync() {
    $(".delete-permission-btn").on("click", async function (e) {
        e.preventDefault();
        await questionDeletePermissionAsync($(this).data("id"));
    });
}
async function questionDeletePermissionAsync(id) {
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
            await deletePermissionAsync(id);
        }
    })
}
async function deletePermissionAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/permission/delete/${id}`,
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
            await resetPermissionDatatableAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await deletePermissionAsync(id);
            }
        }
    });
}

// edit Permission
async function setEditPermissionBtnClickAsync() {
    $(".edit-permission-btn").on("click", async function (e) {
        e.preventDefault();
        await editPermissionAsync($(this).data("id"));
    });
}
async function editPermissionAsync(id) {
    $.ajax({
        url: `${ApiServer}/api/permission/get/${id}`,
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
            await showEditPermissionFormAsync(data.data);
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await editPermissionAsync(id);
            }
        }
    });
}
async function showEditPermissionFormAsync(permission) {
    $("#editPermissionModal").modal("show");
    $("#editPermissionForm").trigger("reset");
    $("#editPermissionForm").data("id",permission.id);
    $("#editPermissionForm input[name=editPermissionName]").val(permission.name);
}
async function closeEditPermissionFormAsync() {
    $("#editPermissionForm button[type=reset]").click();
}
async function getEditPermissionInputAsync() {
    return JSON.stringify({
        name: $("#editPermissionForm input[name=editPermissionName]").val(),
    });
}
async function updatePermissionAsync(id) {
    var data = await getEditPermissionInputAsync();
    console.log(data);
    $.ajax({
        url: `${ApiServer}/api/permission/update/${id}`,
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
            await resetPermissionDatatableAsync();
            await closeEditPermissionFormAsync();
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await updatePermissionAsync();
            }
        }
    });
}





// datatable
function generateActionBtnHtml(id) {
    var html = `<div class="dropdown">
        <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical"></i></button>
        <div class="dropdown-menu">
            <a class="dropdown-item edit-permission-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-pencil me-1"></i> Sửa</a>
            <a class="dropdown-item delete-permission-btn" href="javascript:void(0);" data-id="${id}"><i class="ti ti-trash me-1"></i> Xóa</a>
        </div>
    </div>`;
    return html;
}
async function initPermissionDatatableAsync() {
    $("#permissionDatatable").dataTable({
        ajax: {
            url: `${ApiServer}/api/permission/list-datatable`,
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
                    await resetPermissionDatatableAsync();
                }
            }
        },
        processing: true,
        serverSide: true,
        pagingType: 'full_numbers',
        lengthMenu: [10, 25, 50, 100],
        columns: [
            {
                'data': 'id', orderable: false, render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { 'data': 'name' },
            {
                'data': 'isActived', orderable: false, render: function (data, type, row, meta) {
                    return generateActiveHtml(data);
                }
            },
            {
                'data': 'id',orderable: false, render: function (data, type, row, meta) {
                    return generateActionBtnHtml(data);
                }
            },
        ],
        dom: '<"row me-2"<"col-md-2"<"me-3"l>><"col-md-10"<"dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-end flex-md-row flex-column mb-3 mb-md-0"fB>>>t<"row mx-2"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
        language: {
            lengthMenu: "_MENU_",
            search: "",
            searchPlaceholder: "Tìm tên quyền...",
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
                "data-bs-toggle": "modal",
                "data-bs-target": "#addPermissionModal"
            }
            
        }],
        drawCallback: async function () {
            await setDeletePermissionBtnClickAsync();
            await setEditPermissionBtnClickAsync();
        }

        //responsive: {
        //    details: {
        //        display: $.fn.dataTable.Responsive.display.modal({
        //            header: function (e) {
        //                return "Details of " + e.data().full_name
        //            }
        //        }),
        //        type: "column",
        //        renderer: function (e, t, a) {
        //            a = $.map(a, function (e, t) {
        //                return "" !== e.title ? '<tr data-dt-row="' + e.rowIndex + '" data-dt-column="' + e.columnIndex + '"><td>' + e.title + ":</td> <td>" + e.data + "</td></tr>" : ""
        //            }).join("");
        //            return !!a && $('<table class="table"/><tbody />').append(a)
        //        }
        //    }
        //},
    });
}
async function resetPermissionDatatableAsync() {
    $('#permissionDatatable').DataTable().ajax.reload();
} 