const ClientServer = "https://truyenvv.online";
const ApiServer = "https://api.truyenvv.online";
window.blockPage = function () {
    $.blockUI({
        message: '<div class="sk-wave mx-auto"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div>',
        timeout: 10000,
        css: {
            backgroundColor: "transparent",
            border: "0"
        },
        overlayCSS: {
            opacity: .5
        }
    });
}

window.unblockPage = function () {
    $.unblockUI();
}

window.handleErrorReponseAsync = async function (httpObject) {
    if (httpObject.status == 401) {
        var result = await renewAccessTokenAsync();
        console.log(result);
        if (result == null || result == 500) {
            blockPage();
            toastr.error("500");
            return false;
        } else if (result == 401) {
            /*window.location.href = `${ServerName}/admin/auth/login`;*/
            toastr.error("dont reniew access token");
            return false;
        }
        return true;
    }
    if (httpObject.status == 500) {
        blockPage();
        toastr.error(httpObject.responseJSON.message);
        console.log(httpObject);
        return false;
    }
    if (httpObject.status == 400) {
        console.log(Object.entries(httpObject.responseJSON.data));
        Object.entries(httpObject.responseJSON.data).forEach(([key, value]) => {
            showValidateField(key, value);
            console.log(value);
            console.log(key);
        });
        return false;
    }
    return true;
}

window.showValidateField = function (fieldName, errors) {
    $(`.validate${fieldName}`).html("");
    errors.forEach((error) => {
        $(`.validate${fieldName}`).append(`<li>${error}</li>`);
    });
}
window.cleanValidateField = function (formId) {
    $(`${formId} .validate`).html("");
}
window.renewAccessTokenAsync = async function () {
    var result = null;
    $.ajax({
        url: `${ApiServer}/api/auth/renew-access-token`,
        type: 'GET',
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            console.log(data);
            result = data.statusCode;
        },
        error: function (httpObject,error) {
            toastr.error('Error renew access token.');
            console.log(httpObject);
            result = httpObject.status;
        }
    });
    return result;
    
}

window.convertUTCToLocaleString = function (utcStr) {
    console.log(utcStr);
    const utcDate = new Date(utcStr);
    const localeDate = new Date(utcDate.getTime() - (utcDate.getTimezoneOffset() * 60000));
    console.log(localeDate);
    return localeDate.toLocaleString("vi-VN");
}
window.convertUTCToLocaleDateString = function (utcStr) {
    console.log(utcStr);
    const utcDate = new Date(utcStr);
    const localeDate = new Date(utcDate.getTime() - (utcDate.getTimezoneOffset() * 60000));
    console.log(localeDate);
    return localeDate.toLocaleDateString("vi-VN");
}
window.convertUTCToLocalDate = function (utcStr) {
    console.log(utcStr);
    const utcDate = new Date(utcStr);
    const localeDate = new Date(utcDate.getTime() - (utcDate.getTimezoneOffset() * 60000));
    console.log(localeDate);
    return localeDate;
}
window.convertLocaleDateToISO = function (localeDateStr) {
    const date = new Date(localeDateStr);
    return date.toISOString();
}


//window.convertISOToLocaleDate = function (iso) {
//    console.log("time11111: " + iso);
//    const date = new Date(iso);
//    console.log("time22222:" + date.toLocaleString())
//    return date.toLocaleString();
//}
//window.formatUTCToLocaleDateWithoutTimeZone = function (utcString) {
//    console.log("time1: " + utcString);
//    var utc = new Date(utcString);
//    console.log("time2: " + utc.toDateString("vi-VN"));
//    return utc.toDateString("vi-VN");
//}
//window.formatDateToISOTimeWithoutTimeZone = function (dateString) {
//    var date = new Date(dateString);
//    var timestamp = date.getTime() - date.getTimezoneOffset() * 60000;
//    var correctDate = new Date(timestamp);
//    // correctDate.setUTCHours(0, 0, 0, 0); // uncomment this if you want to remove the time
//    return correctDate.toISOString();
//}
window.generateActiveHtml = function (isActived) {
    if (isActived == true) {
        return `<span class="badge bg-label-primary me-1" >Hoạt động</span>`;
    } else {
        return `<span class="badge bg-label-danger me-1" >Đã dừng</span>`;
    }
}
window.debounce = function(func, timeout = 300){
    let timer;
    return (args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}



    