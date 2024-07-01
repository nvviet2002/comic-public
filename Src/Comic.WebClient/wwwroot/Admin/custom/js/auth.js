$(document).ready(function () {
    $("#form-login").on("submit", async function (e) {
        e.preventDefault();
        var data = await getSignInDataAsync();
        $.ajax({
            url: `${ApiServer}/api/auth/login`,
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
                if (data.statusCode == 200) {
                    window.location.href = "/admin/dashboard";
                }
            },
            complete: function () {
                unblockPage();
            },
            error: async function (httpObject, error) {
                await handleErrorReponseAsync(httpObject);
            }
        });
    });
});
async function getSignInDataAsync() {

    return JSON.stringify({
        email: $("input[name=email]").val(),
        password: $("input[name=password]").val(),
    });
}
function TestApi() {
    $.ajax({
        url: `${ApiServer}/api/auth/test`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            console.log("Test success")
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {

        }
    });
}

